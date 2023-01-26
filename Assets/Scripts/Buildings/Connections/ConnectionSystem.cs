using FreschGames.Core.Input;
using Simpolony.Misc;
using Simpolony.State;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class ConnectionSystem : SystemComponent<ConnectionManager>
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }
        ConnectionManager ConnectionManager { get => this.GameData.ConnectionManager; }

        // Calced
        private GameDataInput Input { get => this.GameData.Input; }

        private EventInputValue PrimaryButton { get => this.Input.PrimaryButton; }
        private InputButton SecondaryButton { get => this.Input.SecondaryButton; }

        // Game State
        private GameStateManager StateManager { get => this.GameData.GameStateManager; }
        private GameStateManagerData StateData { get => this.GameData.GameStateManagerData; }
        private GameState State { get => this.StateData.State; }

        Building ConnectOrigin { get; set; }
        LinkPreview LinkPreview { get; set; }


        protected override void OnAwake() { }

        protected override void OnStart() { }

        protected override void OnUpdate() { }


        private void Awake()
        {
            this.LinkPreview = GameObject.Instantiate(this.ConnectionManager.Data.LinkPreview);
            this.LinkPreview.transform.SetParent(this.transform);
            this.LinkPreview.SetRenderPreview(false);

            this.StopConnecting();
        }

        private void Update()
        {
            this.CheckForState();

            if (this.State != GameState.Connecting)
            {
                this.StopPreview();
                return;
            }

            if (this.SecondaryButton.WasPressed)
            {
                this.StopConnecting();
                return;
            }

            if (this.PrimaryButton.WasPressed)
            {
                this.PrimaryButton.WasPressed.Handled = true;

                Building atPointer = this.GetBuilding();

                if (atPointer == null || atPointer == this.ConnectOrigin)
                    return;

                if (this.ConnectOrigin == null)
                {
                    this.StartConnection(atPointer);
                }
                else
                {
                    if (this.LinkPreview.Contains(atPointer))
                    {
                        this.GameData.ConnectionManager.Connect(this.ConnectOrigin.ID, atPointer.ID);
                        this.StopConnecting();
                    }
                }
            }
        }

        private void CheckForState()
        {
            if (this.State != GameState.Idle)
                return;

            if (this.ConnectOrigin != null)
                return;

            if (this.PrimaryButton.WasPressed)
            {
                Building atPointer = this.GetBuilding();

                if (atPointer == null || atPointer == this.ConnectOrigin)
                    return;

                this.StartConnection(atPointer);
            }
        }

        private void StartConnection(Building atPointer)
        {
            this.StateManager.SetState(GameState.Connecting);

            this.ConnectOrigin = atPointer;

            this.LinkPreview.transform.position = this.ConnectOrigin.transform.position;
            this.LinkPreview.gameObject.SetActive(true);
        }

        private void StopConnecting()
        {
            this.StateManager.SetState(GameState.Idle);

            this.StopPreview();
        }

        private void StopPreview()
        {
            this.ConnectOrigin = null;
            this.LinkPreview.gameObject.SetActive(false);
        }

        private Building GetBuilding()
        {
            var hitColliders = Physics2D.OverlapCircleAll(this.GameData.GameCameraData.WorldPosition, .25f, this.GameData.BuildingLayer);

            // Debug.Log($"Found {hitColliders.Length} collider at {this.GameData.GameCameraData.WorldPosition}");

            float bestDistance = 0;
            Building toSelect = null;

            foreach (var collider in hitColliders)
            {
                Building building = collider.transform.GetProxyComponent<Building>();

                if (building == null)
                    continue;

                float distance = collider.ClosestPoint(this.GameData.GameCameraData.WorldPosition).sqrMagnitude;

                if (toSelect == null || distance < bestDistance)
                {
                    toSelect = building;
                    bestDistance = distance;
                }
            }

            return toSelect;
        }

    }

}