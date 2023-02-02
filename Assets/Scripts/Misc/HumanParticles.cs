using UnityEngine;

namespace Simpolony.Misc
{
    public class HumanParticles : MonoBehaviour
    {
        [field: SerializeField] private ParticleSystem ParticleSystem { get; set; }

        private void Start()
        {
            var main = this.ParticleSystem.main;
            main.stopAction = ParticleSystemStopAction.Destroy;

            this.ParticleSystem.Play();
            this.ParticleSystem.transform.SetParent(null);

            GameObject.Destroy(this.gameObject);
        }
    }
}
