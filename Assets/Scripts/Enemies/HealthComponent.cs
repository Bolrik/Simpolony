using System;
using UnityEngine;

namespace Misc
{
    public abstract class HealthComponent
    {
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }

        public int Missing { get => this.MaxHealth - this.Health; }

        public abstract void Hurt(int damage);
        public abstract void Heal(int damage);
    }

    public class HealthComponent<T> : HealthComponent
        where T : IHealthObject
    {
        private T HealthObject { get; set; }

        public Action<T> OnDestroyed { get; set; }
        public Action<T, int> OnHurt { get; set; }
        public Action<T, int> OnHeal { get; set; }

        public bool IsAlive { get; private set; }


        public void Link(T healthObject)
        {
            this.HealthObject = healthObject;
            this.Health = this.MaxHealth = this.HealthObject.GetMaxHealth();

            this.IsAlive = true;
        }

        public override void Hurt(int damage)
        {
            if (!this.IsAlive)
                Debug.Log("Object already destroyed!");

            damage = damage.Abs();

            this.Health -= damage;

            if (this.Health <= 0)
            {
                this.OnDestroyed?.Invoke(this.HealthObject);

                this.IsAlive = false;

                this.OnHurt = null;
                this.OnDestroyed = null;
            }
            else
            {
                this.OnHurt?.Invoke(this.HealthObject, damage);
            }
        }

        public override void Heal(int heal)
        {
            if (!this.IsAlive)
                Debug.Log("Object already destroyed!");

            heal = heal.Abs();

            this.Health = (this.Health + heal).ClampMax(this.MaxHealth);
            this.OnHeal?.Invoke(this.HealthObject, heal);
        }
    }

    public interface IHealthObject
    {
        HealthComponent Health { get; }
        int GetMaxHealth();
    }

}