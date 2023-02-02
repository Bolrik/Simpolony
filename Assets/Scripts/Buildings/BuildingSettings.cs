using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "Building Settings", menuName = "Data/Settings/Buildings/new Building Settings")]
    public class BuildingSettings : ScriptableObject
    {
        [field: SerializeField, Header("UI")] public Color FloatingTextHurtColor { get; private set; }
        [field: SerializeField] public Color FloatingTextHealColor { get; private set; }
    }
}