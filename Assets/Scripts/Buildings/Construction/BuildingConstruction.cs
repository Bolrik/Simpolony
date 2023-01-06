using System;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class BuildingConstruction : MonoBehaviour
    {
        [field: SerializeField, Header("Data")] public BuildingData Data { get; private set; }

        [field: SerializeField, Header("Info")] public Building Building { get; private set; }


        [field: SerializeField] public int Required { get; private set; }
        [field: SerializeField] public int Delivered { get; private set; }

        public bool IsDone { get => this.Delivered >= this.Required; }


        public void Deliver(int amount)
        {
            this.Delivered += amount;

            if (this.Delivered >= this.Required)
            {
                this.Building.enabled = true;
                Debug.Log("Building construction complete!");
            }

            this.UpdateDeliveryState();

            if (this.IsDone)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        public void Link(Building building, BuildingData data)
        {
            this.Building = building;

            this.Data = data;
            this.Required = this.Data.ResourceCost;

            this.transform.SetParent(this.Building.transform, false);

            if (!this.IsDone)
            {
                this.Building.enabled = false;
            }

            this.UpdateDeliveryState();
        }

        private void UpdateDeliveryState()
        {
            Vector3 scale = Vector3.one;

            if (this.Required > 0)
            {
                float percentDone = (0f + this.Delivered) / this.Required;
                percentDone = .5f + (percentDone * .5f);
                scale *= percentDone;
            }

            this.Building.BuildingVisuals.Transform.localScale = scale;
        }


        public void Destroy()
        {
            
        }
    }
}