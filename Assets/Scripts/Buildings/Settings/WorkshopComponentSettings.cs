using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "Workshop Component Settings", menuName = "Data/Settings/Buildings/new Workshop Component Settings")]
    public class WorkshopComponentSettings : BuildingActionComponentSettings
    {
        [field: SerializeField, Header("Stats")] public float CheckDelay { get; private set; } = 4;
        [field: SerializeField] public float CheckDelayBoost { get; private set; }

        [field: SerializeField] public float HealDelay { get; private set; } = 2;
        [field: SerializeField] public float HealDelayBoost { get; private set; }

        [field: SerializeField] public int HealValue { get; private set; } = 1;
        [field: SerializeField] public float HealValueBoost { get; private set; }
    }
}