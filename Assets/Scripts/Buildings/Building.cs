using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class Building : MonoBehaviour
    {
        public static int IDCounter { get; private set; }

        [field: SerializeField] public BuildingVisualsComponent BuildingVisuals { get; private set; }

        [field: SerializeField] public int ID { get; private set; }

        private void Awake()
        {
            this.ID = Building.IDCounter;
            Building.IDCounter++;

            BuildingsManager.Instance.Add(this.ID, this);
        }

        private void Update()
        {

        }

        public void OnDestroy()
        {
            BuildingsManager.Instance.Remove(this.ID);
        }

        [System.Serializable]
        public class BuildingVisualsComponent
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public MeshRenderer Renderer { get; private set; }
        }
    }

    // Building Mapping Table
    public class BuildingsManager
    {
        #region Singleton Pattern
        public static BuildingsManager Instance { get; private set; }
        static BuildingsManager()
        {
            new BuildingsManager();
        }

        private BuildingsManager()
        {
            Instance = this;
        }
        #endregion

        public Dictionary<int, Building> Buildings { get; private set; } = new Dictionary<int, Building>();

        public void Add(int id, Building building)
        {
            if (this.Buildings.ContainsKey(id))
            {
                Debug.Log("Potential error detected.");
            }

            this.Buildings[id] = building;
        }

        public void Remove(int id)
        {
            if (this.Buildings.ContainsKey(id))
            {
                this.Buildings.Remove(id);
            }
        }

        public Building Get(int id)
        {
            if (!this.Buildings.ContainsKey(id))
                return null;

            return this.Buildings[id];
        }
    }
}