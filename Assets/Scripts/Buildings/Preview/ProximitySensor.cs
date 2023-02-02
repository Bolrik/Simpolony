using FreschGames.Core.Misc;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class ProximitySensor : MonoBehaviour
    {
        [field: SerializeField, Header("Components")] private ProximityColliderComponent ProximityCollider { get; set; }

        List<Transform> Available { get; set; } = new List<Transform>();

        public int Count { get => this.Available.Count; }


        [field: SerializeField] public bool UseTrigger { get; private set; }
        [field: SerializeField] public bool UseTrigger2D { get; private set; }
        [field: SerializeField] public bool UseCollision { get; private set; }
        [field: SerializeField] public bool UseCollision2D { get; private set; }


        public Action<Transform> OnEnter { get; set; }
        public Action<Transform> OnExit { get; set; }

        private void Awake()
        {
            this.ProximityCollider.ProxyCollider.OnTriggerEnter2DEvent += this.OnProxyTriggerEnter2D;
            this.ProximityCollider.ProxyCollider.OnTriggerExit2DEvent += this.OnProxyTriggerExit2D;

            this.ProximityCollider.ProxyCollider.OnTriggerEnterEvent += this.OnProxyTriggerEnter;
            this.ProximityCollider.ProxyCollider.OnTriggerExitEvent += this.OnProxyTriggerExit;

            this.ProximityCollider.ProxyCollider.OnCollisionEnter2DEvent += this.OnProxyCollisionEnter2D;
            this.ProximityCollider.ProxyCollider.OnCollisionExit2DEvent += this.OnProxyCollisionExit2D;

            this.ProximityCollider.ProxyCollider.OnCollisionEnterEvent += this.OnProxyCollisionEnter;
            this.ProximityCollider.ProxyCollider.OnCollisionExitEvent += this.OnProxyCollisionExit;
        }

        public bool Contains(Transform transform)
        {
            return this.Available.Contains(transform);
        }

        private void OnProxyTriggerEnter2D(Collider2D collision)
        {
            if (!this.UseTrigger2D)
                return;

            this.Available.Add(collision.transform);
            this.OnEnter?.Invoke(collision.transform);
        }
        private void OnProxyTriggerExit2D(Collider2D collision)
        {
            if (!this.UseTrigger2D)
                return;

            this.Available.Remove(collision.transform);
            this.OnExit?.Invoke(collision.transform);
        }


        private void OnProxyTriggerEnter(Collider collision)
        {
            if (!this.UseTrigger)
                return;

            this.Available.Add(collision.transform);
            this.OnEnter?.Invoke(collision.transform);
        }
        private void OnProxyTriggerExit(Collider collision)
        {
            if (!this.UseTrigger)
                return;

            this.Available.Remove(collision.transform);
            this.OnExit?.Invoke(collision.transform);
        }


        private void OnProxyCollisionEnter2D(Collision2D collision)
        {
            if (!this.UseCollision2D)
                return;

            this.Available.Add(collision.transform);
            this.OnEnter?.Invoke(collision.transform);
        }
        private void OnProxyCollisionExit2D(Collision2D collision)
        {
            if (!this.UseCollision2D)
                return;

            this.Available.Remove(collision.transform);
            this.OnExit?.Invoke(collision.transform);
        }


        private void OnProxyCollisionEnter(Collision collision)
        {
            if (!this.UseCollision)
                return;

            this.Available.Add(collision.transform);
            this.OnEnter?.Invoke(collision.transform);
        }
        private void OnProxyCollisionExit(Collision collision)
        {
            if (!this.UseCollision)
                return;

            this.Available.Remove(collision.transform);
            this.OnExit?.Invoke(collision.transform);
        }


        [System.Serializable]
        class ProximityColliderComponent
        {
            [field: SerializeField] public ProxyCollider ProxyCollider { get; private set; }
        }
    }

    [System.Serializable]
    public class ProximityStorage<T>
        where T : Component
    {
        [field: SerializeField] public ProximitySensor Sensor { get; private set; }

        List<T> Available { get; set; } = new List<T>();
        public int Count { get => this.Available.Count; }

        bool IsActive { get; set; }

        public void Start()
        {
            if (this.IsActive)
                return;

            this.Available.Clear();

            this.Sensor.OnEnter += this.OnEnter;
            this.Sensor.OnExit += this.OnExit;

            this.IsActive = true;
        }

        public void Stop()
        {
            if (!this.IsActive)
                return;

            this.Sensor.OnEnter -= this.OnEnter;
            this.Sensor.OnExit -= this.OnExit;

            this.IsActive = false;
        }

        public bool Contains(T t)
        {
            return this.Available.Contains(t);
        }

        private void OnEnter(Transform transform)
        {
            if (!(transform.GetProxyComponent<T>() is T match))
                return;

            this.Available.Add(match);
        }

        private void OnExit(Transform transform)
        {
            if (!(transform.GetProxyComponent<T>() is T match))
                return;

            this.Available.Remove(match);
        }

        public T[] GetAll() => this.Available.ToArray();
    }
}