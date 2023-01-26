using Simpolony.Buildings;
using Simpolony.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Resources
{
    [CreateAssetMenu(fileName = "UnitManager", menuName = "Data/Resources/new Unit Manager")]
    public class UnitManager : ManagerComponent
    {
        [field: SerializeField] public int Current { get; private set; }
        [field: SerializeField] public int Capacity { get; private set; }


        [field: SerializeField] private GameData GameData { get; set; }


        public int Available { get => this.Capacity - this.Current; }


        private BuildingManager BuildingManager { get => this.GameData.BuildingManager; }


        public Dictionary<int, bool> BuildingState { get; private set; } = new Dictionary<int, bool>();

        public override void DoAwake()
        {
            this.Current = 0;
            this.Capacity = 0;
        }

        public override void DoStart()
        {
            this.BuildingManager.OnAdded += this.BuildingManager_OnAdded;
            this.BuildingManager.OnRemoved += this.BuildingManager_OnRemoved;

            foreach (var building in this.BuildingManager.Buildings)
            {
                this.Add(building.Value);
                this.SetState(building.Value, building.Value.IsActive);
            }
        }

        public override void DoUpdate()
        {

        }

        private void BuildingManager_OnAdded(Building building)
        {
            this.Add(building);
            this.SetState(building, true);
        }

        private void BuildingManager_OnRemoved(Building building)
        {
            this.Remove(building);
            this.SetState(building, false);
            
            if (this.BuildingState.ContainsKey(building.ID))
            {
                this.BuildingState.Remove(building.ID);
            }
        }

        private void SetState(Building building, bool state)
        {
            if (!this.BuildingState.TryGetValue(building.ID, out bool currentState))
            {
                Debug.Log($"New {building.ID}");
                this.BuildingState[building.ID] = currentState = false;
            }

            bool stateChanged = currentState != state;


            if (stateChanged)
            {
                Debug.Log($"State Changed {building.ID}: {state}");
                this.BuildingState[building.ID] = state;

                int change = building.Data.HousingInfo.CapacityBoost;

                if (state)
                {
                    Debug.Log($"Limit Changed {building.ID}: {change}");
                    this.Capacity += change;
                }
                else
                {
                    Debug.Log($"Limit Changed {building.ID}: {-change}");
                    this.Capacity -= change;
                }

                Debug.Log($"New Limit {this.Capacity}");
            }
        }

        private void Add(Building building)
        {
            int cost = building.Data.HousingInfo.Cost;
            this.Current += cost;

            Debug.Log($"Add: {building.ID} ## Cost: {cost} >> Current: {this.Current}");

            building.OnActiveStateChanged += this.Building_OnActiveStateChanged;
            building.OnDataChanged += this.Building_OnDataChanged;
        }

        private void Remove(Building building)
        {
            int cost = building.Data.HousingInfo.Cost;
            this.Current -= cost;

            Debug.Log($"Rem: {building.ID} ## Cost: {cost} >> Current: {this.Current}");

            building.OnActiveStateChanged -= this.Building_OnActiveStateChanged;
            building.OnDataChanged -= this.Building_OnDataChanged;
        }

        private void Building_OnActiveStateChanged(Building building, bool state)
        {
            this.SetState(building, state);
        }
        

        private void Building_OnDataChanged(Building building, BuildingData previous, BuildingData current)
        {
            if (!this.BuildingState.TryGetValue(building.ID, out bool state))
            {
                state = false;
            }

            int deltaCost = previous.HousingInfo.Cost - current.HousingInfo.Cost;
            int deltaCapacity = previous.HousingInfo.CapacityBoost - current.HousingInfo.CapacityBoost;

            this.Current -= deltaCost;

            if (state)
            {
                this.Capacity -= deltaCapacity;
            }
        }

    }
}