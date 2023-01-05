using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingManagerData", menuName = "Data/Buildings/new Building Manager Data")]
    public class BuildingManagerData : ScriptableObject
    {
        [field: SerializeField] public BuildingPreview Preview { get; private set; }

    }
}