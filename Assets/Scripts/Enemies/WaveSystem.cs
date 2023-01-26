using Simpolony;
using Simpolony.Buildings;
using Simpolony.Misc;
using UnityEngine;

namespace Enemies
{
    public class WaveSystem : SystemComponent<WaveManager>
    {

        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }
        [field: SerializeField] private WaveManagerData WaveManagerData { get; set; }


        private BuildingManager BuildingManager { get => this.GameData.BuildingManager; }


        protected override void OnAwake()
        {

        }

        protected override void OnStart()
        {

        }

        protected override void OnUpdate()
        {
            this.CalculateParameters();
        }

        private void CalculateParameters()
        {
            Vector3 centerPosition = Vector3.zero;

            if (this.BuildingManager.Buildings.Count == 0)
            {
                this.WaveManagerData.SpawnPoint = centerPosition;
                this.WaveManagerData.MinimumSpawnDistance = 0;

                return;
            }

            foreach (var building in this.BuildingManager.Buildings)
            {
                centerPosition += building.Value.transform.position;
            }

            centerPosition /= this.BuildingManager.Buildings.Count;

            float distance = 0;

            foreach (var building in this.BuildingManager.Buildings)
            {
                float buildingDistance = (centerPosition - building.Value.transform.position).magnitude;

                if (buildingDistance > distance)
                    distance = buildingDistance;
            }

            this.WaveManagerData.SpawnPoint = centerPosition;
            this.WaveManagerData.MinimumSpawnDistance = distance;
        }
    }

}