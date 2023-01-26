using FreschGames.Core.Input;
using Simpolony.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simpolony
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField, Header("Data")] private GameData Data { get; set; }


        [field: SerializeField] private bool Trigger { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            this.Data.GameStateManager.SetState(GameState.Idle);
            this.Data.ResourceManager.SetResources(10);
        }

        // Update is called once per frame
        void Update()
        {
            if (this.Trigger)
            {
                this.Trigger = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}