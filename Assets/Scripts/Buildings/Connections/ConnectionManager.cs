using FreschGames.Core.Input;
using Simpolony.Misc;
using Simpolony.Selection;
using Simpolony.State;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "ConnectionManager", menuName = "Data/Buildings/new Connection Manager")]
    public class ConnectionManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] public ConnectionManagerData Data { get; private set; }
        [field: SerializeField] public GameData GameData { get; private set; }

        public Dictionary<int, List<int>> Connections { get; set; } = new Dictionary<int, List<int>>();
        public List<Connection> VisualConnections { get; set; } = new List<Connection>();

        GameObject ConnectionGameObject { get; set; }

        private BuildingManager BuildingManager { get => this.GameData.BuildingManager; }
        private SelectionManager SelectionManager { get => this.GameData.SelectionManager; }

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

        GameObject ContentGameObject { get; set; }


        public override void DoAwake()
        {
            this.ContentGameObject = new GameObject("Connection Manager Content");

            this.LinkPreview = GameObject.Instantiate(this.Data.LinkPreview);
            this.LinkPreview.transform.SetParent(this.ContentGameObject.transform);
            this.LinkPreview.SetRenderPreview(false);

            this.StopConnecting();
        }
        public override void DoStart() { }
        public override void DoUpdate()
        {
            this.CheckForState();

            if (this.State != GameState.Connecting || this.ConnectOrigin?.IsDestroyed == true)
            {
                this.StopPreview();
                return;
            }

            if (this.SecondaryButton.WasReleased)
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
                        this.Connect(this.ConnectOrigin.ID, atPointer.ID);
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




        public void Connect(params int[] toConnect)
        {
            for (int i = 0; i < toConnect.Length; i++)
            {
                for (int j = i + 1; j < toConnect.Length; j++)
                {
                    this.AddConnection(toConnect[i], toConnect[j]);
                }
            }
        }

        private void AddConnection(int origin, int target)
        {
            if (!this.Connections.ContainsKey(origin))
            {
                this.Connections[origin] = new List<int>();
            }
            if (!this.Connections.ContainsKey(target))
            {
                this.Connections[target] = new List<int>();
            }

            if (!this.Connections[origin].Contains(target) && !this.Connections[target].Contains(origin))
            {
                // Debug.Log($"New Connection: {origin} {target}");

                Building buildingOrigin = this.BuildingManager.Get(origin);
                Building buildingTarget = this.BuildingManager.Get(target);

                //if (!this.CheckDistance(buildingOrigin, buildingTarget))
                //    return;

                this.Connections[origin].Add(target);
                this.Connections[target].Add(origin);

                if (this.ConnectionGameObject == null)
                    this.ConnectionGameObject = new GameObject("Connections");

                Connection connection = Instantiate(this.Data.Connection, this.ConnectionGameObject.transform);
                connection.SetTargets(buildingOrigin, buildingTarget);

                this.VisualConnections.Add(connection);
            }
        }

        public void Disconnect(params int[] toConnect)
        {
            for (int i = 0; i < toConnect.Length; i++)
            {
                for (int j = i + 1; j < toConnect.Length; j++)
                {
                    this.RemoveConnection(toConnect[i], toConnect[j]);
                }
            }
        }

        private void RemoveConnection(int origin, int target)
        {
            if (this.Connections.ContainsKey(origin) && this.Connections.ContainsKey(target))
            {
                if (this.Connections[origin].Contains(target) && this.Connections[target].Contains(origin))
                {
                    // Debug.Log($"Removed Connection: {origin} {target}");

                    this.Connections[origin].Remove(target);
                    this.Connections[target].Remove(origin);

                    var connectionToRemove =
                        this.VisualConnections.Where(x =>
                        (x.OriginID == origin && x.TargetID == target) ||
                        (x.OriginID == target && x.TargetID == origin)).FirstOrDefault();


                    if (connectionToRemove != null)
                    {
                        // Debug.Log("Remove Visual Connection");
                        this.VisualConnections.Remove(connectionToRemove);
                        Destroy(connectionToRemove.gameObject);
                    }
                    else
                    {
                        // Debug.Log("No Visual Connection found");
                    }
                }
            }
        }

        public void DisconnectAll(int id)
        {
            if (this.Connections.ContainsKey(id))
            {
                int length = this.Connections[id].Count;

                for (int i = length - 1; i >= 0; i--)
                {
                    this.RemoveConnection(id, this.Connections[id][i]);
                }

                // Debug.Log($"Remaining {this.Connections[id].Count}");
            }
        }
    }
}