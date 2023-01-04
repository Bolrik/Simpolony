using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "Game Camera Data", menuName = "Data/Camera/new Game Camera Data")]
    public class GameCameraData : ScriptableObject
    {
        [field: SerializeField, Header("Out")] public Vector2 WorldPosition { get; set; }
    }
}