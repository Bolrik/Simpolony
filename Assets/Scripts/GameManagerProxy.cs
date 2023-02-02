using Simpolony.UI;
using UnityEngine;

namespace Simpolony
{
    /// <summary>
    ///  Quick n VERY dirty...
    /// </summary>
    public class GameManagerProxy : MonoBehaviour
    {
        [field: SerializeField] public GameScreen GameScreen { get; private set; }
    }
}