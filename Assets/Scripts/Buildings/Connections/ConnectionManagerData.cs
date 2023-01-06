using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "ConnectionManagerData", menuName = "Data/Buildings/new Connection Manager Data")]
    public class ConnectionManagerData : ScriptableObject
    {
        [field: SerializeField, Header("Prefabs")] public Connection Connection { get; private set; }
    }
}