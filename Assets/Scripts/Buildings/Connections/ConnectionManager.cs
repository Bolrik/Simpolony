using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "ConnectionManager", menuName = "Data/Buildings/new Connection Manager")]
    public class ConnectionManager : ScriptableObject
    {
        [field: SerializeField, Header("Data")] private ConnectionManagerData Data { get; set; }

        public Dictionary<int, List<int>> Connections { get; set; } = new Dictionary<int, List<int>>();
        public List<Connection> VisualConnections { get; set; } = new List<Connection>();
        public GameObject ConnectionGameObject { get; set; }

        public void Connect(params int[] toConnect)
        {
            for (int i = 0; i < toConnect.Length; i++)
            {
                for (int j = i + 1; j < toConnect.Length; j++)
                {
                    this.AddConnection(i, j);
                }
            }
        }

        private void AddConnection(int from, int to)
        {
            Debug.Log($"Try Connection: {from} >> {to}");

            if (!this.Connections.ContainsKey(from))
            {
                this.Connections[from] = new List<int>();
            }
            if (!this.Connections.ContainsKey(to))
            {
                this.Connections[to] = new List<int>();
            }

            if (!this.Connections[from].Contains(to) && !this.Connections[to].Contains(from))
            {
                Debug.Log("New Connection");

                this.Connections[from].Add(to);
                this.Connections[to].Add(from);

                if (this.ConnectionGameObject == null)
                    this.ConnectionGameObject = new GameObject("Connections");

                Connection connection = Instantiate(this.Data.Connection, this.ConnectionGameObject.transform);
                connection.SetIDs(from, to);

                this.VisualConnections.Add(connection);
            }
        }

        public void Disconnect(params int[] toConnect)
        {
            for (int i = 0; i < toConnect.Length; i++)
            {
                for (int j = i + 1; j < toConnect.Length; j++)
                {
                    this.RemoveConnection(i, j);
                    this.RemoveConnection(j, i);
                }
            }
        }

        private void RemoveConnection(int from, int to)
        {
            if (this.Connections.ContainsKey(from) && this.Connections[from].Contains(to))
            {
                this.Connections[from].Remove(to);

                // Find the Connection prefab for the from and to node IDs
                Connection connection = this.VisualConnections.FirstOrDefault(x => x.OriginID == from && x.TargetID == to);

                // Remove the Connection prefab from the list and destroy it
                if (connection != null)
                {
                    this.VisualConnections.Remove(connection);
                    Destroy(connection.gameObject);
                }
            }
        }

    }
}