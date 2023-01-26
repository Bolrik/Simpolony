using Simpolony.Misc;
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


        public override void DoAwake() { }
        public override void DoStart() { }
        public override void DoUpdate() { }


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

        public bool CheckDistance(Building origim, Building target)
        {
            float distance = (origim.transform.position - target.transform.position).magnitude;

            return distance <= this.GameData.ConnectionDistanceMax;
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

    [CreateAssetMenu(fileName = "SelectionManager", menuName = "Data/Selection/new Selection Manager")]
    public class SelectionManager : ManagerComponent
    {
        public override void DoAwake()
        {

        }

        public override void DoStart()
        {

        }

        public override void DoUpdate()
        {

        }
    }
}