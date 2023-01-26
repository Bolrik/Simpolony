using Simpolony.Resources;
using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingConstructionManagerData", menuName = "Data/Buildings/new Building Construction Manager Data")]
    public class BuildingConstructionManagerData : ScriptableObject
    {
        [field: SerializeField, Header("Prefabs")] public Building Building { get; private set; }
        [field: SerializeField] public BuildingConstruction Construction { get; private set; }
        [field: SerializeField] public BuildingPreview Preview { get; private set; }


        [field: SerializeField, Header("References")] public ResourceManager ResourceManager { get; private set; }

    }
}