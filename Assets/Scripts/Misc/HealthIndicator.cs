using Misc;
using UnityEngine;

namespace Simpolony.Misc
{
    public class HealthIndicator : MonoBehaviour
    {
        [field: SerializeField] private HealthComponent Health { get; set; }
        [field: SerializeField] private Transform Pivot { get; set; }

        [field: SerializeField] private GameObject VisualsContainer { get; set; }

        [field: SerializeField] private MeshRenderer FillRenderer { get; set; }

        [field: SerializeField] private Gradient Color { get; set; }

        public void Link(HealthComponent healthComponent, Transform parent)
        {
            this.Health = healthComponent;
            this.transform.SetParent(parent, false);
            this.transform.localPosition = new Vector3(0, .8f, -5f);
        }

        private void Update()
        {
            if (this.Health == null || this.Health.Health == this.Health.MaxHealth)
            {
                this.VisualsContainer.SetActive(false);
                return;
            }

            this.VisualsContainer.SetActive(true);

            Vector3 scale = this.Pivot.localScale;
            float t = this.Health.Health / (1f + this.Health.MaxHealth);
            scale.x = t;
            this.Pivot.localScale = scale;

            this.FillRenderer.material.color = this.Color.Evaluate(t);
        }
    }
}
