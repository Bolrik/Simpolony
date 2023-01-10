using Simpolony.Buildings;
using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Game/new Game Data")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField] public bool ShowValidBuildingPreviewBlockedCheck { get; private set; }

        [field: SerializeField] public BuildingConstructionManager BuildingConstructionManager { get; private set; }

        [field: SerializeField] public ConnectionManager ConnectionManager { get; private set; }

        [field: SerializeField] public GameCameraData GameCameraData { get; private set; }

        [field: SerializeField] public bool IsBuilding { get; set; }

    }
}