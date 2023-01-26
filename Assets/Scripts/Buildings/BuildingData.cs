using UnityEngine;

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
            None
        }
    }

    [System.Serializable]
    public class HousingInfo
    {
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public int CapacityBoost { get; private set; }
    }
}