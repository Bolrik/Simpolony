using FreschGames.Core.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony
{
    public class GameCamera : MonoBehaviour
    {
        [field: SerializeField, Header("In")] private Camera Camera { get; set; }
        [field: SerializeField] private InputValue ViewPosition { get; set; }

        [field: SerializeField] private GameCameraData Data { get; set; }


        void Update()
        {
            this.Data.WorldPosition = this.Camera.ScreenToWorldPoint(this.ViewPosition.Read<Vector2>());
        }
    }
}