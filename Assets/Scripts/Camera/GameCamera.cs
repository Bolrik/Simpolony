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

        [field: SerializeField, Header("Out")] public Vector2 WorldPosition { get; private set; }


        // Update is called once per frame
        void Update()
        {
            this.WorldPosition = this.Camera.ScreenToWorldPoint(this.ViewPosition.Read<Vector2>());
            Debug.Log(this.WorldPosition);
        }
    }
}