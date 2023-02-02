using Misc;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simpolony.Projectiles
{
    public class Rocket : MonoBehaviour
    {
        [field: SerializeField, Header("Data")] public GameData GameData { get; private set; }

        [field: SerializeField, Header("References")] public ParticleSystem SmokeParticleSystem { get; private set; }
        [field: SerializeField] public ParticleSystem ExplosionParticleSystem { get; private set; }

        public GameFaction Faction { get; private set; }

        private Vector3 Origin { get; set; }
        private Vector3 Target { get; set; }

        private ITarget TargetObject { get; set; }
        public bool IsDestroyed { get; private set; }



        private float LaunchForce { get; set; } = 1.1337f;

        private float AngularVelocity { get; set; }
        private float Velocity { get; set; }

        private float LaunchDrag { get; set; } = 4f;
        private float AngularDrag { get; set; } = 400f;

        private float RotationSpeed { get; set; } = 2f;

        private int Damage { get; set; }


        private Vector3 Direction { get; set; }

        public Action OnImpact { get; set; }
        public Action OnDestroy { get; set; }

        float AutoKillTimer { get; set; }


        private RocketStage Stage { get; set; }


        public void SetTarget(ITarget target, Vector3 origin, GameFaction faction, float launchForceMultiplier, int damage)
        {
            this.TargetObject = target;
            this.Target = this.TargetObject.GetTargetPosition();

            this.Origin = origin;
            this.Faction = faction;

            this.Damage = damage;

            Vector3 delta = this.Target - this.Origin;
            Vector2 deltaNormal = delta.normalized;

            Vector3 launchDirection = Vector2.Perpendicular(deltaNormal);

            this.Direction = launchDirection;

            this.Velocity = (Random.value - .5f).Sign() * this.LaunchForce * (.9f + Random.value * .4f) * launchForceMultiplier.ClampMin(1);
            this.AngularVelocity = (100 + Random.value * 100) * (Random.value - .5f).Sign();

            this.transform.position = this.Origin;
        }

        private void Update()
        {
            if (this.TargetObject.IsAlive)
            {
                this.Target = this.TargetObject.GetTargetPosition();
            }
            else
            {
                if (this.AutoKillTimer <= 0)
                {
                    this.AutoKillTimer = Time.time + 2 + Random.value * 2;
                }

                if (Time.time >= this.AutoKillTimer)
                {
                    this.Destroy();
                }
            }

            switch (this.Stage)
            {
                case RocketStage.IsLaunch:
                    this.Launch();
                    break;
                case RocketStage.IsLockOn:
                    this.LockOn();
                    break;
                case RocketStage.IsAttack:
                    this.Attack();
                    break;
                default:
                    break;
            }

            this.transform.position += this.Direction * this.Velocity * Time.deltaTime;
            this.transform.localEulerAngles += Vector3.forward * this.AngularVelocity * Time.deltaTime;
        }

        private void Launch()
        {
            // Reduce Velocity to 0
            this.Velocity = Mathf.MoveTowards(this.Velocity, 0, Time.deltaTime * this.LaunchDrag);

            if (this.Velocity == 0)
            {
                this.Stage = RocketStage.IsLockOn;
            }
        }

        private void LockOn()
        {
            if (this.AngularVelocity != 0)
            {
                // Reduce Angular Velocity to 0
                this.AngularVelocity = Mathf.MoveTowards(this.AngularVelocity, 0, Time.deltaTime * this.AngularDrag);
            }
            else
            {
                this.AdjustRotation(out float deltaAngle);

                if (deltaAngle < 40f)
                {
                    this.Stage = RocketStage.IsAttack;
                    this.Velocity = 2f;
                    this.SmokeParticleSystem.Play();
                }
            }
        }

        private void AdjustRotation(out float deltaAngle)
        {
            float angle = 0;
            if (this.TargetObject.IsAlive)
            {
                Vector3 direction = this.Target - this.transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }
            else
            {
                angle = Random.value * 720 - 360;
            }

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, this.RotationSpeed * Time.deltaTime);

            deltaAngle = Quaternion.Angle(this.transform.rotation, rotation);
        }

        private void Attack()
        {
            this.RotationSpeed += Time.deltaTime * 5.25f;

            this.Velocity += Time.deltaTime * 4f;
            this.Velocity = this.Velocity.ClampMax(5f);

            this.AdjustRotation(out _);

            this.Direction = this.transform.right;

            if ((this.transform.position - this.Target).magnitude < .33f)
            {
                this.OnImpact?.Invoke();

                this.DealDamage();
                this.Destroy();
            }
        }

        private void DealDamage()
        {
            if (this.TargetObject.IsAlive)
                this.TargetObject.Health.Hurt(this.Damage);
        }

        public void Destroy()
        {
            if (this.IsDestroyed)
                return;

            this.GameData.GameCameraData.Shake();
            this.IsDestroyed = true;

            var explosion = this.ExplosionParticleSystem.main;
            explosion.stopAction = ParticleSystemStopAction.Destroy;

            this.ExplosionParticleSystem.Play();
            this.ExplosionParticleSystem.transform.SetParent(null);

            if (this.SmokeParticleSystem.isPlaying)
            {
                var main = this.SmokeParticleSystem.main;
                main.stopAction = ParticleSystemStopAction.Destroy;
                main.loop = false;

                this.SmokeParticleSystem.transform.SetParent(null);
            }

            this.OnDestroy?.Invoke();
            GameObject.Destroy(this.gameObject);
        }

        public enum RocketStage
        {
            IsLaunch,
            IsLockOn,
            IsAttack
        }
    }
    //{
    //    public Vector2 StartingPosition { get; set; }
    //    public Vector3 TargetPosition { get; set; }
    //    public ITarget Target { get; set; }

    //    float Velocity { get; set; }

    //    public float LaunchDistance { get; set; } = 1f;
    //    public float RotationSpeed { get; set; } = 10f;
    //    public float FlightSpeed { get; set; } = 10f;

    //    public Action OnImpact { get; set; }

    //    RocketStage Stage { get; set; } = RocketStage.IsLaunch;

    //    private void Start()
    //    {
    //        this.Velocity = this.LaunchDistance;
    //    }

    //    public void Initialize(ITarget target, Vector2 startPosition, Action onImpact)
    //    {
    //        this.StartingPosition = startPosition;
    //        this.Target = target;
    //        this.OnImpact = onImpact;
    //        this.transform.position = this.StartingPosition;
    //    }

    //    private void Update()
    //    {
    //        if (this.Target.IsAlive)
    //            this.TargetPosition = this.Target.GetTargetPosition();

    //        switch (this.Stage)
    //        {
    //            case RocketStage.IsLaunch:
    //                this.Launch();
    //                break;
    //            case RocketStage.IsRotate:
    //                this.RotateTowardsTarget();
    //                break;
    //            case RocketStage.IsAttack:
    //                this.FlyTowardsTarget();
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    private void Launch()
    //    {
    //        if (Vector2.Distance(this.transform.position, this.StartingPosition) < this.LaunchDistance)
    //        {
    //            this.transform.position = Vector2.MoveTowards(this.transform.position, 
    //                this.StartingPosition + (Vector2)this.transform.up * this.LaunchDistance, this.FlightSpeed * Time.deltaTime);
    //            this.transform.localEulerAngles += new Vector3(0, 0, 10 * Time.deltaTime);
    //        }
    //        else
    //        {
    //            this.Stage = RocketStage.IsRotate;
    //        }
    //    }

    //    private void RotateTowardsTarget()
    //    {
    //        Vector3 direction = this.TargetPosition - this.transform.position;
    //        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, this.RotationSpeed * Time.deltaTime);
    //        if (Quaternion.Angle(this.transform.rotation, rotation) < 0.1f)
    //        {
    //            this.Stage = RocketStage.IsAttack;
    //        }
    //    }

    //    private void FlyTowardsTarget()
    //    {
    //        if (Vector2.Distance(this.transform.position, this.TargetPosition) > 0.1f)
    //        {
    //            this.transform.position = Vector2.MoveTowards(this.transform.position, this.TargetPosition, this.FlightSpeed * Time.deltaTime);
    //        }
    //        else
    //        {
    //            this.OnImpact?.Invoke();
    //            GameObject.Destroy(this.gameObject);
    //        }
    //    }

    //    private enum RocketStage
    //    {
    //        IsLaunch,
    //        IsRotate,
    //        IsAttack
    //    }
    //}

    public interface ITarget : IHealthObject
    {
        bool IsAlive { get; }
        Vector3 GetTargetPosition();
    }
}
