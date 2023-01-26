using Simpolony.Projectiles;
using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "PrefabCatalog", menuName = "Data/new Prefab Catalog")]
    public class PrefabCatalog : ScriptableObject
    {
        [field: SerializeField] public Rocket Rocket { get; private set; }
    }
}