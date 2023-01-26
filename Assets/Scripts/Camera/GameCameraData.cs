using FreschGames.Core.Misc;
using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "Game Camera Data", menuName = "Data/Camera/new Game Camera Data")]
    public class GameCameraData : ScriptableObject
    {
        [field: SerializeField] public float CameraSpeed { get; private set; }
        [field: SerializeField] public float MouseCameraSpeed { get; private set; }
        [field: SerializeField] public float ZoomSpeed { get; private set; } = .5f;
        [field: SerializeField] public float ZoomSpeedModifier { get; private set; } = .01f;

        [field: SerializeField] public MinMax<float> ZoomConstraints { get; private set; }

        public Vector2 WorldPosition { get; set; }
        
    }
}