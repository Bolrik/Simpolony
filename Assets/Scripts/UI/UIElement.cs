using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        [field: SerializeField] public UIDocument Document { get; private set; }

        Action OnUpdate { get; set; }

        public void Register(Action action)
        {
            this.OnUpdate += action;
        }


        private void Update()
        {
            this.OnUpdate?.Invoke();
        }
    }
}
