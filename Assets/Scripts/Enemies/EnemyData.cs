using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/Enemies/new Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField, Header("Stats")] public int MaxHealth { get; private set; }
        [field: SerializeField] public float HealthPercentPerLevel { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }

        [field: SerializeField, Header("Attack")] public int Damage { get; private set; }
        [field: SerializeField] public float DamagePercentPerLevel { get; private set; }
        [field: SerializeField] public float LaunchForceMultiplier { get; private set; }
        [field: SerializeField] public float AttackDelay { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }

        [field: SerializeField, Header("Score")] public int Points { get; private set; }
    }
}