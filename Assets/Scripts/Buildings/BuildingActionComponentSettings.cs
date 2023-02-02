using UnityEngine;

namespace Simpolony.Buildings
{
    public abstract class BuildingActionComponentSettings : ScriptableObject
    {
        [field: SerializeField, Header("Base")] public float ExperiencePerSecond { get; private set; }

    }
}