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
                    this.AddConnection(j, i);
                }
            }
        }

        private void AddConnection(int from, int to)
        {
            if (!Connections.ContainsKey(from))
            {
                this.Connections[from] = new List<int>();
            }
            if (!Connections.ContainsKey(to))
            {
                this.Connections[to] = new List<int>();
            }

            if (!Connections[from].Contains(to) && !Connections[to].Contains(from))
            {
                this.Connections[from].Add(to);
                this.Connections[to].Add(from);

                if (this.ConnectionGameObject == null)
                    this.ConnectionGameObject = new GameObject("Connections");


                Connection connection = Instantiate(this.Data.Connection, this.ConnectionGameObject.transform);
                connection.SetNodes(from, to);
                VisualConnections.Add(connection);
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
            if (Connections.ContainsKey(from) && Connections[from].Contains(to))
            {
                Connections[from].Remove(to);
                Connections[to].Remove(from);

                // Find the Connection prefab for the from and to node IDs
                Connection connection = VisualConnections.FirstOrDefault(x => x.FromNode == from && x.ToNode == to);

                // Remove the Connection prefab from the list and destroy it
                if (connection != null)
                {
                    VisualConnections.Remove(connection);
                    Destroy(connection.gameObject);
                }
            }
        }

    }
}