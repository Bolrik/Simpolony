using System;
using UnityEngine;

namespace Misc
{
    public abstract class HealthComponent
    {
        public abstract void TakeDamage(int damage);
    }

    public class HealthComponent<T> : HealthComponent
        where T : IHealthObject
    {
        private T HealthObject { get; set; }

        public int Health { get; private set; }

        public Action<T> OnDestroyed { get; set; }
        public Action<T, int> OnDamaged { get; set; }

        public bool IsAlive { get; private set; }


        public void Link(T healthObject)
        {
            this.HealthObject = healthObject;
            this.Health = this.HealthObject.GetMaxHealth();

            this.IsAlive = true;
        }

        public override void TakeDamage(int damage)
        {
            if (!this.IsAlive)
                Debug.Log("Object already destroyed!");

            damage = damage.Abs();

            this.Health -= damage;

            if (this.Health <= 0)
            {
                this.OnDestroyed?.Invoke(this.HealthObject);

                this.IsAlive = false;

                this.OnDamaged = null;
                this.OnDestroyed = null;
            }
            else
            {
                this.OnDamaged?.Invoke(this.HealthObject, damage);
            }
        }
    }

    public interface IHealthObject
    {
        HealthComponent Health { get; }
        int GetMaxHealth();
    }

}