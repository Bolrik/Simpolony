using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingPreviewData", menuName = "Data/Buildings/new Building Preview Data")]
    public class BuildingPreviewData : ScriptableObject
    {
        [field: SerializeField] public Color InvalidColor { get; private set; }
        [field: SerializeField] public Color ValidColor { get; private set; }

        [field: SerializeField] public LineRenderer ConnectionRenderer { get; private set; }
    }
}