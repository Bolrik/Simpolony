using Enemies;
using FreschGames.Core.Input;
using Simpolony.Buildings;
using Simpolony.Messages;
using Simpolony.Resources;
using Simpolony.Score;
using Simpolony.Selection;
using Simpolony.State;
using Simpolony.UI.MessagePanelElements;
using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Game/new Game Data")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField, Header("Building Manager")] public BuildingManager BuildingManager { get; private set; }

        [field: SerializeField, Header("Building Construction Manager")] public BuildingConstructionManager BuildingConstructionManager { get; private set; }
        [field: SerializeField] public bool ShowValidBuildingPreviewBlockedCheck { get; private set; }

        [field: SerializeField, Header("Building Delivery Manager")] public BuildingDeliveryManager BuildingDeliveryManager { get; private set; }

        [field: SerializeField, Header("Connection Manager")] public ConnectionManager ConnectionManager { get; private set; }
        [field: SerializeField] public float ConnectionDistanceMax { get; private set; } = 4f;

        [field: SerializeField, Header("Resource Manager")] public ResourceManager ResourceManager { get; private set; }

        [field: SerializeField, Header("Game Camera")] public GameCameraData GameCameraData { get; private set; }

        [field: SerializeField, Header("Game State Manager")] public GameStateManager GameStateManager { get; private set; }
        [field: SerializeField] public GameStateManagerData GameStateManagerData { get; private set; }

        [field: SerializeField, Header("Wave Manager")] public WaveManager WaveManager { get; private set; }

        [field: SerializeField, Header("Selection Manager")] public SelectionManager SelectionManager { get; private set; }
        
        [field: SerializeField, Header("Unit Manager")] public UnitManager UnitManager { get; private set; }

        [field: SerializeField, Header("Message Manager")] public MessageManager MessageManager { get; set; }

        [field: SerializeField, Header("Score Manager")] public ScoreManager ScoreManager { get; set; }


        [field: SerializeField, Header("Settings")] public BuildingSettings BuildingSettings { get; private set; }
        [field: SerializeField] public EnemySettings EnemySettings { get; private set; }

        [field: SerializeField, Header("Layer Mask")] public LayerMask BuildingLayer { get; private set; }
        [field: SerializeField] public LayerMask RocketLayer { get; private set; }

        [field: SerializeField, Header("Input")] public GameDataInput Input { get; private set; }
        [field: SerializeField, Header("Building Action Settings")] public BuildingActionComponentSettingsContainer ActionComponentSettings { get; private set; }


        [field: SerializeField, Header("Prefab Catalog")] public PrefabCatalog PrefabCatalog { get; private set; }
    }

    [System.Serializable]
    public class GameDataInput
    {
        [field: SerializeField, Header("General")] public EventInputValue PrimaryButton { get; private set; }
        [field: SerializeField] public InputButton SecondaryButton { get; private set; }
        [field: SerializeField] public InputValue ScrollValue { get; private set; }
        [field: SerializeField] public InputValue ViewPosition { get; private set; }
        [field: SerializeField] public InputValue MenuButton { get; private set; }
        [field: SerializeField] public InputValue DestroyButton { get; private set; }

        [field: SerializeField, Header("Camera")] public InputValue ResetCamera { get; private set; }
        [field: SerializeField] public InputValue CameraMovement { get; private set; }
        
    }

    [System.Serializable]
    public class BuildingActionComponentSettingsContainer
    {
        [field: SerializeField] public TowerComponentSettings Tower { get; private set; }
        [field: SerializeField] public MineComponentSettings Mine { get; private set; }
        [field: SerializeField] public WorkshopComponentSettings Workshop { get; private set; }
        [field: SerializeField] public BarrierComponentSettings Barrier { get; private set; }
    }
}