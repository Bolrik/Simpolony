using FreschGames.Core.Input;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [field: SerializeField] private BuildingManagerData Data { get; set; }

        [field: SerializeField] private BuildingPreview ActivePreview { get; set; }


        [field: SerializeField] private InputButton LeftClick { get; set; }

        [field: SerializeField] private GameCameraData GameCameraData { get; set; }


        private void Update()
        {
            if (this.ActivePreview == null)
                return;

            this.ActivePreview.SetPosition();

            if (this.LeftClick.WasPressed)
            {

            }
        }

    }

    [CreateAssetMenu(fileName = "BuildingManagerData", menuName = "Data/Buildings/new Building Manager Data")]
    public class BuildingManagerData : ScriptableObject
    {
        [field: SerializeField] public BuildingPreview Preview { get; private set; }

    }
}