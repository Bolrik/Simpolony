using UnityEngine;

namespace Simpolony.Misc
{
    public abstract class SystemComponent<T> : MonoBehaviour
        where T : ManagerComponent
    {
        [field: SerializeField, Header("Manager")] public T Manager { get; private set; }

        private void Awake()
        {
            this.Manager.DoAwake();
            this.OnAwake();
        }

        private void Start()
        {
            this.Manager.DoStart();
            this.OnStart();
        }

        private void Update()
        {
            this.Manager.DoUpdate();
            this.OnUpdate();
        }

        protected abstract void OnAwake();
        protected abstract void OnStart();
        protected abstract void OnUpdate();
    }
}
