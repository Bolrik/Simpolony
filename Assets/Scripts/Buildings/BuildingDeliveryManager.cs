using Simpolony.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "BuildingDeliveryManager", menuName = "Data/Buildings/new Building Delivery Manager")]
    public class BuildingDeliveryManager : ManagerComponent
    {
        List<BuildingDelivery> BuildingDeliveries { get; set; } = new List<BuildingDelivery>();

        public void Add(BuildingConstruction construction)
        {
            BuildingDelivery delivery = new BuildingDelivery(construction);
            delivery.Construction.OnDone += this.Delivery_Construction_OnDone;

            this.BuildingDeliveries.Add(delivery);
        }

        public override void DoAwake()
        {
            this.BuildingDeliveries.Clear();
        }

        public override void DoStart()
        {
            
        }

        public override void DoUpdate()
        {
            BuildingDelivery[] deliveries = this.BuildingDeliveries.ToArray();

            for (int i = 0; i < deliveries.Length; i++)
            {
                BuildingDelivery delivery = deliveries[i];

                if (!delivery.Construction.Building.IsAlive)
                {
                    this.Remove(delivery);
                    continue;
                }

                delivery.StepTime += Time.deltaTime;

                if (delivery.StepTime >= delivery.StepTimer)
                {
                    delivery.Construction.Deliver(1);
                    delivery.StepTime = 0;
                }
            }

            //for (int i = this.BuildingDeliveries.Count - 1; i >= 0; i--)
            //{
            //    BuildingDelivery delivery = this.BuildingDeliveries[i];

            //    if (!delivery.Construction.Building.IsAlive)
            //    {
            //        this.BuildingDeliveries.RemoveAt(i);
            //        continue;
            //    }

            //    delivery.StepTime += Time.deltaTime;

            //    if (delivery.StepTime >= delivery.StepTimer)
            //    {
            //        delivery.Construction.Deliver(1);
            //        delivery.StepTime = 0;
            //    }

            //    if (delivery.Construction.IsDone)
            //    {
            //        this.BuildingDeliveries.RemoveAt(i);
            //    }
            //}
        }

        private void Remove(BuildingConstruction construction)
        {
            for (int i = 0; i < this.BuildingDeliveries.Count; i++)
            {
                var delivery = this.BuildingDeliveries[i];

                if (delivery.Construction == construction)
                {
                    // this.BuildingDeliveries.RemoveAt(i);
                    this.Remove(delivery);
                    return;
                }
            }
        }

        private void Remove(BuildingDelivery delivery)
        {
            delivery.Construction.OnDone -= this.Delivery_Construction_OnDone;

            this.BuildingDeliveries.Remove(delivery);
        }

        private void Delivery_Construction_OnDone(BuildingConstruction construction)
        {
            this.Remove(construction);
        }
    }
}