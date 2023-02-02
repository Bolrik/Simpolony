using FreschGames.Core.Misc;
using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "Tower Component Settings", menuName = "Data/Settings/Buildings/new Tower Component Settings")]
    public class TowerComponentSettings : BuildingActionComponentSettings
    {
        [field: SerializeField, Header("Stats")] public float AttackDelay { get; private set; } = 1f;
        [field: SerializeField] public float AttackDelayBoost { get; private set; }

        [field: SerializeField] public int AttackDamage { get; private set; } = 1;
        [field: SerializeField] public float AttackDamageBoost { get; private set; }
    }
}