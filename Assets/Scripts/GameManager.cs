using FreschGames.Core.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] private InputValue ViewPosition { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(this.ViewPosition.Read<Vector2>());
        }
    }
}