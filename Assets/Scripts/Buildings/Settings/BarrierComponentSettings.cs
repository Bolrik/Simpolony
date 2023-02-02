using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "Barrier Component Settings", menuName = "Data/Settings/Buildings/new Barrier Component Settings")]
    public class BarrierComponentSettings : BuildingActionComponentSettings
    {
        [field: SerializeField, Header("Stats")] public float CheckDelay { get; private set; } = 1.5f;
        [field: SerializeField] public float CheckDelayBoost { get; private set; } = -.1f;

        [field: SerializeField] public float DisarmDelay { get; private set; } = 0.75f;
        [field: SerializeField] public float DisarmDelayBoost { get; private set; } = -0.05f;
    }
}