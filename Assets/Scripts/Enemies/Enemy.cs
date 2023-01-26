﻿using Misc;
using Simpolony;
using Simpolony.Buildings;
using Simpolony.Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IHealthObject, ITarget
    {

        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }
        [field: SerializeField] public EnemyData Data { get; private set; }

        [field: SerializeField, Header("Components")] private EnemyVisionComponent Vision { get; set; }
        [field: SerializeField] private EnemyVisualComponent Visuals { get; set; }

        private Vector3 Destination { get; set; }
        private Building Target { get; set; }
        private EnemyState State { get; set; }

        public float AttackCooldown { get; private set; }

        public int Level { get; private set; }
        public int Damage { get; private set; }

        public HealthComponent<Enemy> Health { get; private set; }
        HealthComponent IHealthObject.Health => this.Health;

        Rocket Rocket { get; set; }


        public bool IsAlive { get => this.Health.IsAlive; }


        private void Start()
        {
            this.SetData(this.Data, 0);

            this.State = new MoveState(this);
            this.Vision.Storage.Start();
        }

        private void Update()
        {
            this.State.Tick();
        }

        public void SetData(EnemyData data, int level)
        {
            this.Data = data;
            this.Level = level;

            this.CalculateDamage();

            this.Vision.Transform.localScale = Vector3.one * this.Data.AttackRange;
            this.Visuals.SetColor(this.Data.Color);
            
            this.InitHealth();
        }

        private void InitHealth()
        {
            if (this.Health == null)
            {
                this.Health = new HealthComponent<Enemy>();
                this.Health.Link(this);
                this.Health.OnDestroyed += this.Health_OnDestroyed;
            }
            else
            {
                this.Health.Link(this);
            }
        }

        public void SetDestination(Vector3 destination)
        {
            this.Destination = destination;
            this.State = new MoveState(this);
        }

        private void Health_OnDestroyed(Enemy enemy)
        {
            this.gameObject.SetActive(false);
            this.Rocket?.Destroy();
            GameObject.Destroy(this.gameObject);
        }

        private void MoveToDestination()
        {
            Vector3 direction = (this.Destination - this.transform.position).normalized;
            float speed = this.Data.MovementSpeed;
            this.transform.Translate(direction * speed * Time.deltaTime);
        }

        private void CheckForBuildingsInProximity()
        {
            if (this.Vision.Storage.Count > 0)
            {
                //this.Target = this.Vision.Storage.GetAll()[0];
                //this.Attacking = true;
                //this.StartAttacking();

                float minDistance = float.MaxValue;
                Building closestBuilding = null;

                foreach (var building in this.Vision.Storage.GetAll())
                {
                    float distance = Vector3.Distance(this.transform.position, building.transform.position);
                    
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestBuilding = building;
                    }
                }

                if (minDistance < this.Data.AttackRange)
                {
                    this.Target = closestBuilding;
                    this.StartAttacking();
                }
            }
        }

        private void StartAttacking()
        {
            this.State = new AttackState(this);


            this.AttackCooldown = 0;
        }

        public void Attack()
        {
            if (this.AttackCooldown <= Time.time && this.Rocket == null)
            {
                this.ResetAttackTime();

                this.StartRocket();
                // this.DamageTarget();
            }
        }

        private void StartRocket()
        {
            this.Rocket = GameObject.Instantiate(this.GameData.PrefabCatalog.Rocket);
            this.Rocket.SetTarget(this.Target, this.transform.position, this.Data.LaunchForceMultiplier, this.Damage);
            this.Rocket.OnImpact += this.Rocket_OnDestroy;
        }

        private void Rocket_OnDestroy()
        {
            this.Rocket.OnImpact -= this.Rocket_OnDestroy;
            this.Rocket = null;
        }

        private void DamageTarget()
        {
            if (this.Target != null)
                this.Target.Health.TakeDamage(this.Data.Damage);
        }

        public void CheckAttack()
        {
            if (this.Target != null && this.Target.Health.IsAlive)
            {
                return;
            }

            Debug.Log("Target Destroyed!");

            this.Target = null;
            this.State = new MoveState(this);
        }

        private void ResetAttackTime()
        {
            this.AttackCooldown = Time.time + this.Data.AttackDelay;
        }

        public int GetMaxHealth()
        {
            return (int)((this.Data?.MaxHealth ?? 1) * (1 + this.Level * this.Data.HealthPercentPerLevel));
        }

        private void CalculateDamage()
        {
            this.Damage = (int)(this.Data.Damage * (1 + this.Level * this.Data.DamagePercentPerLevel));
        }


        Vector3 ITarget.GetTargetPosition()
        {
            return this.transform.position;
        }

        private abstract class EnemyState
        {
            protected Enemy Enemy { get; set; }

            public EnemyState(Enemy enemy) { this.Enemy = enemy; }
            public abstract void Tick();
        }

        private class MoveState : EnemyState
        {
            public MoveState(Enemy enemy) : base(enemy) { }

            public override void Tick()
            {
                this.Enemy.MoveToDestination();
                this.Enemy.CheckForBuildingsInProximity();
            }
        }

        private class AttackState : EnemyState
        {
            public AttackState(Enemy enemy) : base(enemy)
            {

            }

            public override void Tick()
            {
                this.Enemy.Attack();
                this.Enemy.CheckAttack();
            }
        }

        [System.Serializable]
        public class EnemyVisionComponent
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public ProximityStorage<Building> Storage { get; private set; }
        }

        [System.Serializable]
        public class EnemyVisualComponent
        {
            [field: SerializeField] public MeshRenderer Renderer { get; private set; }

            public void SetColor(Color color)
            {
                this.Renderer.material.color = color;
            }
        }
    }
}