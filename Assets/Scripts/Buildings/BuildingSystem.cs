using Simpolony.Misc;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class BuildingSystem : SystemComponent<BuildingManager>
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        protected override void OnAwake() { }
    
        protected override void OnStart() { }

        protected override void OnUpdate() { }
        //BuildingManager BuildingManager { get => this.GameData.BuildingManager; }

        //private void Update()
        //{
        //    this.BuildingManager.Update();
        //}
    }
}