using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Game/new Game Data")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField] public bool ShowValidBuildingPreviewBlockedCheck { get; private set; }
    }
}