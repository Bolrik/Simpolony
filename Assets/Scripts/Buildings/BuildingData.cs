using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Data/Buildings/new Building Data")]
    public class BuildingData : ScriptableObject
    {
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public int ResourceCost { get; private set; }
        [field: SerializeField] public float ConstructionStepTime { get; private set; }
    }
}