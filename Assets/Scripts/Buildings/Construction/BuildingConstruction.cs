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

        public bool IsDone { get; private set; }

        public Action<BuildingConstruction> OnDone { get; set; }


        public void Deliver(int amount)
        {
            if (this.IsDone)
                return;

            this.Delivered += amount;

            if (this.Delivered >= this.Required)
            {
                // this.Building.enabled = true;
                this.Building.SetActive();

                Debug.Log($"'{this.Data}' >> Building construction complete!");

                this.SetDone();
            }

            this.UpdateDeliveryState();
        }

        private void SetDone()
        {
            this.IsDone = true;
            this.UpdateDeliveryState();

            this.OnDone?.Invoke(this);

            this.Destroy();
        }

        public void Link(Building building, BuildingData data)
        {
            this.Building = building;

            this.Data = data;
            this.Required = this.Data.ResourceCost;

            this.Building.SetData(this.Data);

            this.transform.SetParent(this.Building.transform, false);
            this.Building.SetInactive();

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
            this.OnDone = null;

            GameObject.Destroy(this.gameObject);
        }
    }
}