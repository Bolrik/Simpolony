using FreschGames.Core.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }
        [field: SerializeField] private BuildingManagerData Data { get; set; }

        [field: SerializeField, Header("Input")] private InputButton PrimaryButton { get; set; }
        [field: SerializeField] private InputButton SecondaryButton { get; set; }

        [field: SerializeField, Header("Info")] private BuildingPreview ActivePreview { get; set; }


        [field: SerializeField, Header("Debug")] private BuildingData ToBuild { get; set; }


        List<BuildingDelivery> ActiveDeliveries { get; set; } = new List<BuildingDelivery>();


        private void Update()
        {
            for (int i = this.ActiveDeliveries.Count - 1; i >= 0; i--)
            {
                BuildingDelivery delivery = this.ActiveDeliveries[i];

                delivery.Time += Time.deltaTime;

                if (delivery.Time >= delivery.Timer)
                {
                    delivery.Construction.Deliver(1);
                    delivery.Time = 0;
                }

                if (delivery.Construction.IsDone)
                {
                    this.ActiveDeliveries.RemoveAt(i);
                }
            }

            this.CheckBuild();
        }

        private void CheckBuild()
        {
            this.GameData.IsBuilding = this.ActivePreview != null;

            if (this.ActivePreview == null)
            {
                if (this.PrimaryButton.WasPressed)
                {
                    this.ActivePreview = GameObject.Instantiate(this.Data.Preview);
                    this.UpdatePreviewPosition();
                    return;
                }
                else
                    return;
            }

            if (this.SecondaryButton.WasPressed)
            {
                this.StopPreview();
                return;
            }

            this.UpdatePreviewPosition();

            if (this.ActivePreview.IsValid && this.PrimaryButton.WasPressed)
            {
                this.ConfirmPreview();
            }
        }

        private void ConfirmPreview()
        {
            Building connectionTarget = this.ActivePreview.DesiredLink;

            this.StopPreview();

            BuildingConstruction construction = this.GameData.BuildingConstructionManager.StartConstruction(this.ToBuild);

            if (construction != null)
            {
                construction.Building.transform.position = this.GameData.GameCameraData.WorldPosition;

                this.GameData.ConnectionManager.Connect(connectionTarget.ID, construction.Building.ID);

                BuildingDelivery delivery = new BuildingDelivery(construction);
                this.ActiveDeliveries.Add(delivery);
            }
        }

        private void StopPreview()
        {
            this.ActivePreview.Destroy();
            this.ActivePreview = null;
        }

        private void UpdatePreviewPosition()
        {
            this.ActivePreview.SetPosition(this.GameData.GameCameraData.WorldPosition);
        }

        class BuildingDelivery
        {
            [field: SerializeField] public BuildingConstruction Construction { get; private set; }

            [field: SerializeField] public float Time { get; set; }
            public float Timer { get => this.Construction.Data.ConstructionStepTime; }


            public BuildingDelivery(BuildingConstruction construction)
            {
                this.Construction = construction;
            }
        }
    }
}