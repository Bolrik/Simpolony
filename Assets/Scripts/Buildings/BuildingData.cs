using UnityEngine;
using static Simpolony.Buildings.BuildingData;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Data/Buildings/new Building Data")]
    public class BuildingData : ScriptableObject
    {
        [field: SerializeField, Header("Construction")] public int ResourceCost { get; private set; }
        [field: SerializeField] public float ConstructionStepTime { get; private set; }

        [field: SerializeField, Header("General")] public int Health { get; private set; }
        [field: SerializeField] public BuildingDataType DataType { get; private set; }
        [field: SerializeField] public int VisionRange { get; private set; }
        [field: SerializeField] public int AbilityRange { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }

        [field: SerializeField, Header("Housing")] public HousingInfo HousingInfo { get; private set; }

        [field: SerializeField, Header("UI")] public Sprite MenuSprite { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }




        public BuildingActionComponent GetActionComponent()
        {
            switch (this.DataType)
            {
                case BuildingDataType.Core:
                case BuildingDataType.Platform:
                case BuildingDataType.House:
                case BuildingDataType.None:
                    return null;
                case BuildingDataType.BarrierBeacon:
                    return new BarrierBeaconComponent();
                case BuildingDataType.Workshop:
                    return new WorkshopComponent();
                case BuildingDataType.Mine:
                    return new MineComponent();
                case BuildingDataType.Tower:
                    return new TowerComponent();
                default:
                    break;
            }

            return null;
        }

        public enum BuildingDataType
        {
            Core,
            Platform,
            Mine,
            Tower,
            House,
            None,
            Workshop,
            BarrierBeacon
        }
    }

    public static class BuildingDataTypeExtension
    {
        public static string GetDisplayName(this BuildingDataType buildingDataType)
        {
            switch (buildingDataType)
            {
                case BuildingDataType.Core:
                    return "Core";
                case BuildingDataType.Platform:
                    return "Wall";
                case BuildingDataType.Mine:
                    return "Mine";
                case BuildingDataType.Tower:
                    return "Tower";
                case BuildingDataType.House:
                    return "Expansion Hub";
                case BuildingDataType.None:
                    return "MissingNo.";
                case BuildingDataType.Workshop:
                    return "Workshop";
                case BuildingDataType.BarrierBeacon:
                    return "Barrier Beacon";
                default:
                    break;
            }

            return "";
        }
    }

    [System.Serializable]
    public class HousingInfo
    {
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public int CapacityBoost { get; private set; }
    }
}