using UnityEngine;

namespace Simpolony.Buildings
{
    [System.Serializable]
    public class BuildingDelivery
    {
        [field: SerializeField] public BuildingConstruction Construction { get; private set; }

        [field: SerializeField] public float StepTime { get; set; }
        public float StepTimer { get => this.Construction.Data.ConstructionStepTime; }


        public BuildingDelivery(BuildingConstruction construction)
        {
            this.Construction = construction;
        }
    }
}