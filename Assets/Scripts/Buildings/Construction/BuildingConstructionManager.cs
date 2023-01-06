using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingConstructionManager", menuName = "Data/Buildings/new Building Construction Manager")]
    public class BuildingConstructionManager : ScriptableObject
    {
        [field: SerializeField] private BuildingConstructionManagerData Data { get; set; }

        public BuildingConstruction StartConstruction(BuildingData buildingData)
        {
            int resourceCost = buildingData.ResourceCost;

            if (this.Data.ResourceManager.GetResourceCount() >= resourceCost)
            {
                this.Data.ResourceManager.UseResources(resourceCost);

                Building building = Instantiate(this.Data.Building);
                BuildingConstruction construction = Instantiate(this.Data.Construction);
                //BuildingConstruction construction = building.gameObject.AddComponent<BuildingConstruction>();
                construction.Link(building, buildingData);

                return construction;
            }
            else
            {
                Debug.Log("Not enough resources to start construction.");
            }

            return null;
        }
    }
}