using Enemies;
using Misc;
using Simpolony.Misc;
using Simpolony.Projectiles;
using Simpolony.Resources;
using Simpolony.Selection;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simpolony.Buildings
{
    public class Building : MonoBehaviour, IHealthObject, ITarget, ISelectable
    {
        public static int IDCounter { get; private set; }

        [field: SerializeField, Header("Data")] public GameData GameData { get; private set; }
        [field: SerializeField] public BuildingData Data { get; private set; }

        [field: SerializeField, Header("Componentes")] public BuildingVisualsComponent BuildingVisuals { get; private set; }
        [field: SerializeField] public BuildingVisionComponent Vision { get; private set; }

        [field: SerializeField, Header("Info")] public int ID { get; private set; }

        public HealthComponent<Building> Health { get; private set; }
        HealthComponent IHealthObject.Health => this.Health;

        public bool IsAlive { get => this.Health.IsAlive; }
        bool ITarget.IsAlive => this.IsAlive;

        bool ISelectable.IsSelectable { get => this.Health.IsAlive; }

        private Vector3 ValidPosition { get; set; }


        public BuildingActionComponent ActionComponent { get; private set; }
        private BuildingManager BuildingManager { get => this.GameData.BuildingManager; }

        public bool IsActive { get => this.enabled; }
        public bool IsDestroyed { get; private set; }

        float NextHumanParticleTime { get; set; }

        // Events
        public Action<Building, bool> OnActiveStateChanged { get; set; }
        public Action<Building, BuildingData, BuildingData> OnDataChanged { get; set; }

        int PreviousLevel { get; set; } = 0;

        private void Awake()
        {
            this.ID = Building.IDCounter;
            Building.IDCounter++;

            this.SetData(this.Data);
            this.BuildingVisuals.SetAbilityRangeVisible(false);
            this.BuildingVisuals.SetLevel(0);

            this.BuildingManager.Add(this.ID, this);
            //BuildingsManager.Instance.Add(this.ID, this);

            HealthIndicator healthIndicator = GameObject.Instantiate(this.GameData.PrefabCatalog.HealthIndicator);
            healthIndicator.Link(this.Health, this.transform);

        }

        private void Update()
        {
            this.ValidPosition = this.transform.position;

            this.ActionComponent?.Tick();

            int level = (int)(this.ActionComponent?.Experience ?? 0);
            if (level > this.PreviousLevel)
            {
                this.PreviousLevel = level;
                this.BuildingVisuals.SetLevel(level);
                this.GameData.MessageManager.AddMessage($"Building '{this.Data.DataType.GetDisplayName()}' is now level {level}.");
            }
        }

        public void OnDestroy()
        {
            this.BuildingManager.Remove(this.ID);
            this.GameData.ConnectionManager.DisconnectAll(this.ID);
        }

        public void SetActive()
        {
            this.enabled = true;
            this.OnActiveStateChanged?.Invoke(this, true);
        }

        public void SetInactive()
        {
            this.enabled = false;
            this.OnActiveStateChanged?.Invoke(this, false);
        }

        public void SetData(BuildingData data)
        {
            if (data != null)
            {
                BuildingData previous = this.Data;

                this.Data = data;

                this.UpdateVisuals();

                this.ActionComponent = this.Data.GetActionComponent();
                this.ActionComponent?.Link(this);

                this.Vision.Transform.localScale = Vector3.one * this.Data.VisionRange;
                this.Vision.Enemies.Start();

                this.OnDataChanged?.Invoke(this, previous, this.Data);
            }

            this.InitHealth();
        }

        private void UpdateVisuals()
        {
            this.BuildingVisuals.SetColor(this.Data.Color);
            int range = this.Data.AbilityRange.Max(this.Data.VisionRange);

            this.BuildingVisuals.SetAbilityRangeScale(range);
        }

        private void InitHealth()
        {
            if (this.Health == null)
            {
                this.Health = new HealthComponent<Building>();
                this.Health.Link(this);
                this.Health.OnHurt += this.Health_OnHurt;
                this.Health.OnHeal += this.Health_OnHeal;
                this.Health.OnDestroyed += this.Health_OnDestroyed;
            }
            else
            {
                this.Health.Link(this);
            }
        }

        public int GetMaxHealth()
        {
            return this.Data?.Health ?? 1;
        }

        private void Health_OnDestroyed(Building building)
        {
            this.Destroy();
        }

        private void Health_OnHurt(Building building, int amount)
        {
            FloatingText text = GameObject.Instantiate(this.GameData.PrefabCatalog.FloatingText);
            text.SetText(this.ValidPosition + Vector3.forward * -5,
                $"{amount}", this.GameData.BuildingSettings.FloatingTextHurtColor, Vector3.up + Vector3.right * (Random.value * 2 - 1), 2);

            if (this.Data.DataType != BuildingData.BuildingDataType.Platform && this.enabled && this.NextHumanParticleTime <= Time.time)
            {
                this.NextHumanParticleTime = Time.time + 1f;

                HumanParticles particles = GameObject.Instantiate(this.GameData.PrefabCatalog.HumanParticles);
                particles.transform.position = this.transform.position;
            }
        }

        private void Health_OnHeal(Building building, int amount)
        {
            FloatingText text = GameObject.Instantiate(this.GameData.PrefabCatalog.FloatingText);
            text.SetText(this.ValidPosition + Vector3.forward * -5,
                $"{amount}", this.GameData.BuildingSettings.FloatingTextHealColor, Vector3.up + Vector3.right * (Random.value * 2 - 1), 2);
        }

        public Vector3 GetTargetPosition()
        {
            try
            {
                if (!this.IsAlive)
                    return this.ValidPosition;

                return this.transform.position;

            }
            catch
            {
                return this.ValidPosition;
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = this.ValidPosition = position;
        }

        void ISelectable.SetSelected(bool state)
        {
            if (this.IsDestroyed)
                return;

            if (state)
                Debug.Log(this.ActionComponent?.Experience);

            this.BuildingVisuals.SetAbilityRangeVisible(state);
        }

        public void Destroy()
        {
            if (this.IsDestroyed)
                return;

            this.IsDestroyed = true;

            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }


        [System.Serializable]
        public class BuildingVisualsComponent
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public MeshRenderer Renderer { get; private set; }
            [field: SerializeField] public MeshRenderer AbilityRangeRenderer { get; private set; }


            [field: SerializeField] private MeshRenderer[] LevelUpCubes { get; set; }


            public void SetColor(Color color)
            {
                this.Renderer.material.color = color;
            }

            public void SetLevel(int level)
            {
                for (int i = 0; i < this.LevelUpCubes.Length; i++)
                {
                    this.LevelUpCubes[i].enabled = i < level;
                }
            }

            public void SetAbilityRangeVisible(bool state)
            {
                this.AbilityRangeRenderer.enabled = state;
            }

            public void SetAbilityRangeScale(float scale)
            {
                this.AbilityRangeRenderer.transform.localScale = (Vector3)(Vector2.one * scale) + Vector3.forward * .5f;
            }
        }

        [System.Serializable]
        public class BuildingVisionComponent
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public ProximityStorage<Enemy> Enemies { get; private set; }
        }
    }

    

    public abstract class BuildingActionComponent
    {
        protected Building Building { get; private set; }
        protected PrefabCatalog PrefabCatalog { get => this.Building.GameData?.PrefabCatalog; }
        protected ResourceManager ResourceManager { get => this.Building.GameData?.ResourceManager; }

        public float Experience { get; protected set; }


        public void Link(Building building)
        {
            this.Building = building;
            this.OnLink(building);
        }
        protected abstract void OnLink(Building building);

        protected abstract void OnTick();
        public virtual void Tick()
        {
            this.OnTick();
        }
    }

    public abstract class BuildingActionComponent<T> : BuildingActionComponent
        where T : BuildingActionComponentSettings
    {
        protected abstract T GetSettings();

        public override void Tick()
        {
            this.UpdateExperience();

            base.Tick();
        }
        
        protected void UpdateExperience()
        {
            T settings = this.GetSettings();

            this.Experience += settings.ExperiencePerSecond * Time.deltaTime;
            this.Experience = this.Experience.ClampMax(5);
        }

    }

    public class TowerComponent : BuildingActionComponent<TowerComponentSettings>
    {
        Enemy Target { get; set; }

        float AttackTime { get; set; }
        // float AttackDelay { get; set; } = 1f;
        float AttackDelay { get; set; }
        int AttackDamage { get; set; }

        float LaunchForceMultiplier { get; set; } = 2f;

        protected override void OnLink(Building building)
        {
            this.ResetAttackTime();
        }

        private void CalculateStats()
        {
            var settings = this.GetSettings();

            int level = (int)this.Experience;

            this.AttackDelay = settings.AttackDelay;
            this.AttackDelay += settings.AttackDelayBoost * level;

            this.AttackDamage = settings.AttackDamage;
            this.AttackDamage += (int)(settings.AttackDamageBoost * level);
        }

        protected override void OnTick()
        {
            this.CalculateStats();

            int length = this.Building.Vision.Enemies.Count;

            if (this.Target != null && this.Target.IsAlive)
            {
                if (this.Building.Vision.Enemies.Contains(this.Target))
                {
                    if (this.AttackTime <= Time.time)
                    {
                        if (this.ResourceManager.GetResourceCount() >= 1)
                        {
                            this.ResourceManager.UseResources(1);

                            this.ResetAttackTime();

                            this.LaunchRocket();
                        }
                    }
                }
                else
                {
                    this.Target = null;
                }
            }
            else if (length > 0)
            {
                this.Target = this.Building.Vision.Enemies.GetAll()[Random.Range(0, length)];
            }
        }

        private void ResetAttackTime()
        {
            this.AttackTime = Time.time + this.AttackDelay;
        }

        private void LaunchRocket()
        {
            Rocket rocket = GameObject.Instantiate(this.PrefabCatalog.Rocket);
            rocket.SetTarget(this.Target, this.Building.transform.position, GameFaction.Player, this.LaunchForceMultiplier, this.AttackDamage);
        }

        protected override TowerComponentSettings GetSettings()
        {
            return this.Building.GameData.ActionComponentSettings.Tower;
        }
    }

    public class MineComponent : BuildingActionComponent<MineComponentSettings>
    {
        private float Timer { get; set; }


        private float MineDelay { get; set; } = 5f;

        private int MineValue { get; set; } = 1;


        protected override void OnLink(Building building)
        {
            this.UpdateTimer();
        }


        private void CalculateStats()
        {
            var settings = this.GetSettings();

            int level = (int)this.Experience;

            this.MineDelay = settings.MineDelay;
            this.MineDelay += settings.MineDelayBoost * level;

            this.MineValue = settings.MineValue;
            this.MineValue += (int)(settings.MineValueBoost * level);
        }

        protected override void OnTick()
        {
            this.CalculateStats();

            if (this.UpdateTimer())
            {
                this.Mine();
            }
        }

        private void Mine()
        {
            this.ResourceManager.AddResources(this.MineValue);
        }

        private bool UpdateTimer()
        {
            if (this.Timer <= Time.time)
            {
                this.Timer = Time.time + this.MineDelay;

                return true;
            }

            return false;
        }


        protected override MineComponentSettings GetSettings()
        {
            return this.Building.GameData.ActionComponentSettings.Mine;
        }
    }

    public class BarrierBeaconComponent : BuildingActionComponent<BarrierComponentSettings>
    {
        private float CheckTime { get; set; }

        private float CheckDelay { get; set; }
        private float DisarmDelay { get; set; }

        Rocket Target { get; set; }


        protected override void OnLink(Building building)
        {
            this.UpdateTimer();
        }

        private void CalculateStats()
        {
            var settings = this.GetSettings();

            int level = (int)this.Experience;

            this.CheckDelay = settings.CheckDelay;
            this.CheckDelay += settings.CheckDelayBoost * level;

            this.DisarmDelay = settings.DisarmDelay;
            this.DisarmDelay += settings.DisarmDelayBoost * level;
        }

        protected override void OnTick()
        {
            this.CalculateStats();

            if (this.UpdateTimer())
            {
                this.SetDisarmTarget();
            }

            if (this.Target != null)
            {
                this.Disarm();
                this.CheckTime += this.DisarmDelay;
                this.Target = null;
            }
        }

        private void SetDisarmTarget()
        {
            var nearby = 
                Physics2D.OverlapCircleAll(this.Building.transform.position, this.Building.Data.AbilityRange / 2, this.Building.GameData.RocketLayer);

            List<Rocket> toDisarm = new List<Rocket>();

            foreach (var collider in nearby)
            {
                if (!(collider.transform.GetProxyComponent<Rocket>() is Rocket rocket))
                    continue;

                if (rocket.Faction == GameFaction.Player)
                    continue;

                toDisarm.Add(rocket);
            }

            if (toDisarm.Count == 0)
                return;
            
            if (toDisarm.Count == 1)
            {
                this.Target = toDisarm[0]; ;
            }
            else
            {
                float minDistance = (toDisarm[0].transform.position - this.Building.transform.position).magnitude;
                this.Target = toDisarm[0];

                for (int i = 1; i < toDisarm.Count; i++)
                {
                    Rocket target = toDisarm[i];
                    float distance = (toDisarm[0].transform.position - this.Building.transform.position).magnitude;

                    if (distance > minDistance)
                    {
                        this.Target = target;
                        minDistance = distance;
                    }
                }
            }
        }

        private bool UpdateTimer()
        {
            if (this.CheckTime <= Time.time)
            {
                this.CheckTime = Time.time + this.CheckDelay;

                return true;
            }

            return false;
        }

        public void Disarm()
        {
            DisarmRay ray = GameObject.Instantiate(PrefabCatalog.DisarmRay);
            ray.SetUp(this.Building.transform.position, this.Target, this.DisarmDelay);

            this.Target = null;
        }

        protected override BarrierComponentSettings GetSettings()
        {
            return this.Building.GameData.ActionComponentSettings.Barrier;
        }
    }

    public class WorkshopComponent : BuildingActionComponent<WorkshopComponentSettings>
    {
        private float CheckTime { get; set; }

        private float CheckDelay { get; set; } = 4f;
        private float HealDelay { get; set; } = 2f;

        private int HealValue { get; set; } = 1;

        Building Target { get; set; }


        protected override void OnLink(Building building)
        {
            this.UpdateTimer();
        }

        private void CalculateStats()
        {
            var settings = this.GetSettings();

            int level = (int)this.Experience;

            this.CheckDelay = settings.CheckDelay;
            this.CheckDelay += settings.CheckDelayBoost * level;
            
            this.HealDelay = settings.HealDelay;
            this.HealDelay += settings.HealDelayBoost * level;

            this.HealValue = settings.HealValue;
            this.HealValue += (int)(settings.HealValueBoost * level);
        }

        protected override void OnTick()
        {
            this.CalculateStats();

            if (this.UpdateTimer())
            {
                this.SetHealTarget();
            }

            if (this.Target != null)
            {
                this.Heal();
            }
        }

        private void SetHealTarget()
        {
            var nearby = Physics2D.OverlapCircleAll(this.Building.transform.position, this.Building.Data.AbilityRange / 2, this.Building.GameData.BuildingLayer);

            List<Building> toHeal = new List<Building>();

            foreach (var collider in nearby)
            {
                if (!(collider.transform.GetProxyComponent<Building>() is Building building))
                    continue;

                if (building.Health.Missing <= 0)
                    continue;

                toHeal.Add(building);
            }

            if (toHeal.Count == 0)
                return;

            if (toHeal.Count == 1)
            {
                this.Target = toHeal[0]; ;
            }
            else
            {
                int least = toHeal[0].Health.Missing;
                this.Target = toHeal[0];

                for (int i = 1; i < toHeal.Count; i++)
                {
                    Building target = toHeal[i];
                    int missing = target.Health.Missing;

                    if (missing > least)
                    {
                        this.Target = target;
                        least = missing;
                    }
                }
            }
        }

        private bool UpdateTimer()
        {
            if (this.CheckTime <= Time.time)
            {
                this.CheckTime = Time.time + this.CheckDelay;

                return true;
            }

            return false;
        }

        public void Heal()
        {
            HealBolt healBolt = GameObject.Instantiate(PrefabCatalog.HealBolt);
            healBolt.SetUp(this.Building.transform.position, this.Target.transform.position, this.Target, this.HealDelay, this.HealValue);


            this.Target = null;
        }

        protected override WorkshopComponentSettings GetSettings()
        {
            return this.Building.GameData.ActionComponentSettings.Workshop;
        }
    }
}