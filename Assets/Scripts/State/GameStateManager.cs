using UnityEngine;

namespace Simpolony.State
{
    [CreateAssetMenu(fileName = "Game State Manager", menuName = "Data/State/new Game State Manager")]
    public class GameStateManager : ScriptableObject
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        private GameStateManagerData Data { get => this.GameData.GameStateManagerData; }


        public void SetState(GameState state)
        {
            this.Data.State = state;
        }
    }


    public enum GameState
    {
        Idle,
        Building,
        Connecting
    }
}