using UnityEngine;

namespace Simpolony.Buildings
{
    public class BuildingConstructionManager : MonoBehaviour
    {
        [field: SerializeField] public GameObject BuildingPrefab { get; private set; }

        [field: SerializeField] public int ResourceCost { get; private set; }
        [field: SerializeField] public ResourceManager ResourceManager { get; private set; }

        public void StartConstruction()
        {
            if (this.ResourceManager.GetResourceCount() >= this.ResourceCost)
            {
                this.ResourceManager.UseResources(this.ResourceCost);

                GameObject building = Instantiate(this.BuildingPrefab, this.transform.position, this.transform.rotation);

                BuildingConstruction construction = building.AddComponent<BuildingConstruction>();
            }
            else
            {
                Debug.Log("Not enough resources to start construction.");
            }
        }
    }

    public class BuildingConstruction : MonoBehaviour
    {
        [field: SerializeField] public bool IsConstructing { get; private set; }
        [field: SerializeField] public float ConstructionTime { get; private set; } = 10f;
        [field: SerializeField] public float ConstructionTimer { get; private set; }

        public void StartConstruction()
        {
            this.IsConstructing = true;
        }

        void Update()
        {
            if (this.IsConstructing)
            {
                this.ConstructionTimer += Time.deltaTime;

                if (this.ConstructionTimer >= this.ConstructionTime)
                {
                    this.IsConstructing = false;
                    this.ConstructionTimer = 0f;
                    Debug.Log("Building construction complete!");
                }
            }
        }
    }

    public class ResourceManager : ScriptableObject
    {
        [field: SerializeField] public int Available { get; private set; }

        public int GetResourceCount()
        {
            return this.Available;
        }

        public void UseResources(int amount)
        {
            this.Available -= amount;
        }
    }
}