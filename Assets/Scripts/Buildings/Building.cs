using Enemies;
using Misc;
using Simpolony.Projectiles;
using Simpolony.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class Building : MonoBehaviour, IHealthObject, ITarget
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

        private Vector3 LastValidPosition { get; set; }


        BuildingActionComponent ActionComponent { get; set; }


        private BuildingManager BuildingManager { get => this.GameData.BuildingManager; }
        public bool IsActive { get => this.enabled; }

        // Events
        public Action<Building, bool> OnActiveStateChanged { get; set; }
        public Action<Building, BuildingData, BuildingData> OnDataChanged { get; set; }



        private void Awake()
        {
            this.ID = Building.IDCounter;
            Building.IDCounter++;

            this.SetData(this.Data);

            this.BuildingManager.Add(this.ID, this);

            //BuildingsManager.Instance.Add(this.ID, this);
        }

        private void Update()
        {
            this.LastValidPosition = this.transform.position;

            this.ActionComponent?.Tick();
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
                this.BuildingVisuals.SetColor(this.Data.Color);

                this.ActionComponent = this.Data.GetActionComponent();
                this.ActionComponent?.Link(this);

                this.Vision.Transform.localScale = Vector3.one * this.Data.VisionRange;
                this.Vision.Enemies.Start();

                this.OnDataChanged?.Invoke(this, previous, this.Data);
            }

            this.InitHealth();
        }

        private void InitHealth()
        {
            if (this.Health == null)
            {
                this.Health = new HealthComponent<Building>();
                this.Health.Link(this);
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
            this.gameObject.SetActive(false);
            GameObject.Destroy(building.gameObject);
            // BuildingsManager.Instance.Remove(this.ID);
        }

        public Vector3 GetTargetPosition()
        {
            if (!this.IsAlive)
                return this.LastValidPosition;

            return this.transform.position;
        }

        [System.Serializable]
        public class BuildingVisualsComponent
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public MeshRenderer Renderer { get; private set; }
            [field: SerializeField] public MeshRenderer AbilityRangeRenderer { get; private set; }

            public void SetColor(Color color)
            {
                this.Renderer.material.color = color;
            }

            public void SetAbilityRangeVisible(bool state)
            {
                this.AbilityRangeRenderer.enabled = state;
            }
        }

        [System.Serializable]
        public class BuildingVisionComponent
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public ProximityStorage<Enemy> Enemies { get; private set; }
        }
    }

    //// Building Mapping Table
    //public class BuildingsManager
    //{
    //    #region Singleton Pattern
    //    public static BuildingsManager Instance { get; private set; }
    //    static BuildingsManager()
    //    {
    //        new BuildingsManager();
    //    }

    //    private BuildingsManager()
    //    {
    //        Instance = this;
    //    }
    //    #endregion

    //    public Dictionary<int, Building> Buildings { get; private set; } = new Dictionary<int, Building>();

    //    public void Add(int id, Building building)
    //    {
    //        if (this.Buildings.ContainsKey(id))
    //        {
    //            Debug.Log("Potential error detected.");
    //        }

    //        this.Buildings[id] = building;

    //        Debug.Log($"+ Buildings: {this.Buildings.Count}");
    //    }

    //    public void Remove(int id)
    //    {
    //        if (this.Buildings.ContainsKey(id))
    //        {
    //            var building = this.Buildings[id];
    //            this.Buildings.Remove(id);

    //            Debug.Log($"- Buildings: {this.Buildings.Count}");
    //        }
    //    }

    //    public Building Get(int id)
    //    {
    //        if (!this.Buildings.ContainsKey(id))
    //            return null;

    //        return this.Buildings[id];
    //    }
    //}

    public abstract class BuildingActionComponent
    {
        public int Level { get; private set; }
        public int UpgradeCost { get; private set; }

        protected Building Building { get; private set; }
        protected PrefabCatalog PrefabCatalog { get => this.Building.GameData?.PrefabCatalog; }
        protected ResourceManager ResourceManager { get => this.Building.GameData?.ResourceManager; }


        public void Link(Building building)
        {
            this.Building = building;
            this.OnLink(building);
        }
        protected abstract void OnLink(Building building);

        public abstract void Tick();
    }

    public class TowerComponent : BuildingActionComponent
    {
        Enemy Target { get; set; }
        float AttackTime { get; set; }

        float BaseAttackDelay { get; set; } = 1f;
        float LaunchForceMultiplier { get; set; } = 2f;
        int BaseAttackDamage { get; set; } = 1;

        protected override void OnLink(Building building)
        {
            this.ResetAttackTime();
        }

        public override void Tick()
        {
            if (this.Building.Vision.Enemies.Count > 0)
            {
                if (this.Target == null || !this.Target.IsAlive)
                {
                    this.Target = this.Building.Vision.Enemies.GetAll()[0];
                }
                else
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
            }
        }

        private void ResetAttackTime()
        {
            this.AttackTime = Time.time + this.BaseAttackDelay;
        }

        private void LaunchRocket()
        {
            Rocket rocket = GameObject.Instantiate(this.PrefabCatalog.Rocket);
            rocket.SetTarget(this.Target, this.Building.transform.position, this.LaunchForceMultiplier, this.BaseAttackDamage);
        }

        private void DamageTarget()
        {
            if (this.Target.IsAlive)
                this.Target.Health.TakeDamage(5);

            if (!this.Target.IsAlive)
                this.Target = null;
        }
    }

    public class MineComponent : BuildingActionComponent
    {
        private float Timer { get; set; }


        private float BaseDelay { get; set; } = 5f;

        private int MineValue { get; set; } = 1;



        protected override void OnLink(Building building)
        {
            this.UpdateTimer();
        }

        public override void Tick()
        {
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
                this.Timer = Time.time + this.BaseDelay;

                return true;
            }

            return false;
        }
    }
}