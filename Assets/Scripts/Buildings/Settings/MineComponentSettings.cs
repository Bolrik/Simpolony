using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "Mine Component Settings", menuName = "Data/Settings/Buildings/new Mine Component Settings")]
    public class MineComponentSettings : BuildingActionComponentSettings
    {
        [field: SerializeField, Header("Stats")] public float MineDelay { get; private set; } = 5f;
        [field: SerializeField] public float MineDelayBoost { get; private set; }

        [field: SerializeField] public int MineValue { get; private set; } = 1;
        [field: SerializeField] public float MineValueBoost { get; private set; }
    }
}