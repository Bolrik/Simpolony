using FreschGames.Core.Input;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [field: SerializeField] private BuildingManagerData Data { get; set; }

        [field: SerializeField] private BuildingPreview ActivePreview { get; set; }


        [field: SerializeField] private InputButton PrimaryButton { get; set; }

        [field: SerializeField] private GameCameraData GameCameraData { get; set; }


        private void Update()
        {
            if (this.ActivePreview == null)
            {
                if (this.PrimaryButton.WasPressed)
                {
                    this.ActivePreview = GameObject.Instantiate(this.Data.Preview);
                }
                else
                    return;
            }

            this.ActivePreview.SetPosition(this.GameCameraData.WorldPosition);

            if (this.PrimaryButton.WasPressed)
            {

            }
        }

    }
}