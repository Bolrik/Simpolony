using Simpolony.Misc;
using Simpolony.Projectiles;
using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "PrefabCatalog", menuName = "Data/new Prefab Catalog")]
    public class PrefabCatalog : ScriptableObject
    {
        [field: SerializeField, Header("Projectiles")] public Rocket Rocket { get; private set; }
        [field: SerializeField] public DisarmRay DisarmRay { get; private set; }
        [field: SerializeField] public HealBolt HealBolt { get; private set; }


        [field: SerializeField, Header("Utility")] public FloatingText FloatingText { get; private set; }
        [field: SerializeField] public HealthIndicator HealthIndicator { get; private set; }


        [field: SerializeField, Header("Particle Systems")] public HumanParticles HumanParticles { get; private set; }
    }
}