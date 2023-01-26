using FreschGames.Core.Input;
using Simpolony.Misc;
using Simpolony.Resources;
using Simpolony.State;
using System;
using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingConstructionManager", menuName = "Data/Buildings/new Building Construction Manager")]
    public class BuildingConstructionManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }
        [field: SerializeField] private BuildingConstructionManagerData Data { get; set; }

        [field: SerializeField, Header("Values")] public bool IsActive { get; private set; }

        private BuildingPreview BuildingPreview { get; set; }
        private BuildingData BuildingData { get; set; }

        // Input
        private GameDataInput Input { get => this.GameData.Input; }

        private EventInputValue PrimaryButton { get => this.Input.PrimaryButton; }
        private InputButton SecondaryButton { get => this.Input.SecondaryButton; }

        // Game State
        private GameStateManager StateManager { get => this.GameData.GameStateManager; }
        private GameStateManagerData StateData { get => this.GameData.GameStateManagerData; }
        private GameState State { get => this.StateData.State; }

        // Managers
        private ResourceManager ResourceManager { get => this.GameData.ResourceManager; }
        private UnitManager UnitManager { get => this.GameData.UnitManager; }

        public Action<BuildEvent> OnConstructionStarted { get; set; }


        public override void DoAwake()
        {
            this.OnConstructionStarted = null;
        }

        public override void DoStart()
        {

        }

        public override void DoUpdate()
        {
            if (this.State != GameState.Building)
            {
                return;
            }

            if (this.SecondaryButton.WasPressed)
            {
                this.StopPreview();
                return;
            }

            this.UpdatePreviewPosition();

            if (this.BuildingPreview.IsValid && this.PrimaryButton.WasPressed)
            {
                this.ConfirmPreview();
            }
        }

        public void StartPreview(BuildingData data)
        {
            if (this.State != GameState.Connecting && this.State != GameState.Idle)
                return;

            this.StateManager.SetState(GameState.Building);

            this.PrimaryButton.WasPressed.Handled = true;

            this.BuildingData = data;

            if (this.BuildingPreview == null)
            {
                this.BuildingPreview = GameObject.Instantiate(this.Data.Preview);
            }

            this.BuildingPreview.SetData(data);

            this.UpdatePreviewPosition();
        }

        private void ConfirmPreview()
        {
            Building connectionTarget = this.BuildingPreview.DesiredLink;

            if (this.StartConstruction(out BuildingConstruction construction))
            {
                this.StopPreview();

                construction.Building.transform.position = this.GameData.GameCameraData.WorldPosition;

                Debug.Log($"'{this.BuildingData}' >> Building construction started!");

                this.GameData.ConnectionManager.Connect(connectionTarget.ID, construction.Building.ID);

                this.GameData.BuildingDeliveryManager.Add(construction);
            }
        }

        private void StopPreview()
        {
            this.StateManager.SetState(GameState.Idle);

            this.BuildingPreview.Destroy();
            this.BuildingPreview = null;
        }


        private void UpdatePreviewPosition()
        {
            this.BuildingPreview.SetPosition(this.GameData.GameCameraData.WorldPosition);
        }

        private bool StartConstruction(out BuildingConstruction construction)
        {
            int resourceCost = this.BuildingData.ResourceCost;

            int remainingUnitCapacity = this.UnitManager.Available;
            int cost = this.BuildingData.HousingInfo.Cost;

            bool unitCapacityCheck = cost == 0 || remainingUnitCapacity >= this.BuildingData.HousingInfo.Cost;
            bool resourceCheck = this.ResourceManager.GetResourceCount() >= resourceCost;

            if (unitCapacityCheck && resourceCheck)
            {
                this.ResourceManager.UseResources(resourceCost);

                Building building = Instantiate(this.Data.Building);
                construction = Instantiate(this.Data.Construction);
                construction.Link(building, this.BuildingData);

                this.OnConstructionStarted?.Invoke(new BuildEvent(this.BuildingData, building, construction));

                return true;
            }
            else
            {
                if (!unitCapacityCheck)
                    Debug.Log("Not enough Unit Capacity to start Construction.");
                if (!resourceCheck)
                    Debug.Log("Not enough Resources to start Construction.");
            }

            construction = null;
            return false;
        }
    }
}