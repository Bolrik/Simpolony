using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemy Settings", menuName = "Data/Settings/Enemies/new Enemy Settings")]
    public class EnemySettings : ScriptableObject
    {
        [field: SerializeField, Header("UI")] public Color FloatingTextHurtColor { get; private set; }
        [field: SerializeField] public Color FloatingTextHealColor { get; private set; }
    }
}