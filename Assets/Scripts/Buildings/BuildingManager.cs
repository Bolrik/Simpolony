using FreschGames.Core.Input;
using Simpolony.Misc;
using Simpolony.State;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingManager", menuName = "Data/Buildings/new Building Manager")]
    public class BuildingManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }
        [field: SerializeField] private BuildingManagerData Data { get; set; }


        public Dictionary<int, Building> Buildings { get; private set; } = new Dictionary<int, Building>();
        public Action<Building> OnAdded { get; set; }
        public Action<Building> OnRemoved { get; set; }

        public override void DoAwake()
        {
            this.Buildings.Clear();
        }

        public override void DoStart()
        {
            
        }

        public override void DoUpdate()
        {

        }




        public void Add(int id, Building building)
        {
            if (this.Buildings.ContainsKey(id))
            {
                Debug.Log("Potential error detected.");
            }
            
            this.Buildings[id] = building;
            this.OnAdded?.Invoke(building);
        }

        public void Remove(int id)
        {
            if (this.Buildings.ContainsKey(id))
            {
                var building = this.Buildings[id];
                this.Buildings.Remove(id);
                this.OnRemoved?.Invoke(building);
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