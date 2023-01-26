using FreschGames.Core.Input;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class Connection : MonoBehaviour
    {
        [field: SerializeField, Header("References")] private LineRenderer LineRenderer { get; set; }

        [field: SerializeField, Header("Info")] public Building Origin { get; private set; }
        [field: SerializeField] public Building Target { get; private set; }

        public int OriginID { get; private set; }
        public int TargetID { get; private set; }

        //public void SetIDs(int originID, int targetID)
        //{
        //    this.OriginID = originID;
        //    this.Origin = BuildingsManager.Instance.Get(this.OriginID);

        //    this.TargetID = targetID;
        //    this.Target = BuildingsManager.Instance.Get(this.TargetID);

        //    this.UpdateVisuals();
        //}

        public void SetTargets(Building origin, Building target)
        {
            this.Origin = origin;
            this.OriginID = this.Origin.ID;

            this.Target = target;
            this.TargetID = this.Target.ID;

            this.UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            this.LineRenderer.SetPosition(0, this.Origin.transform.position);
            this.LineRenderer.SetPosition(1, (this.Origin.transform.position + this.Target.transform.position) / 2f);
            this.LineRenderer.SetPosition(2, this.Target.transform.position);
        }
    }
}