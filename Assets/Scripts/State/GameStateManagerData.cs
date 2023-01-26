using UnityEngine;

namespace Simpolony.State
{
    [CreateAssetMenu(fileName = "Game State Manager Data", menuName = "Data/State/new Game State Manager Data")]
    public class GameStateManagerData : ScriptableObject
    {
        [field: SerializeField] public GameState State { get; set; }
    }
}