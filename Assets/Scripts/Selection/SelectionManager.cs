using FreschGames.Core.Input;
using Simpolony.Misc;
using System;
using UnityEngine;

namespace Simpolony.Selection
{
    [CreateAssetMenu(fileName = "SelectionManager", menuName = "Data/Selection/new Selection Manager")]
    public class SelectionManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        // Input
        private GameDataInput Input { get => this.GameData.Input; }

        private EventInputValue PrimaryButton { get => this.Input.PrimaryButton; }
        private InputButton SecondaryButton { get => this.Input.SecondaryButton; }


        public Action<SelectionEvent> OnSelection { get; set; }

        public ISelectable Hover { get; private set; }
        public ISelectable Selected { get; private set; }


        public override void DoAwake()
        {
            this.Hover = null;
            this.Selected = null;
        }

        public override void DoStart()
        {

        }

        public override void DoUpdate()
        {
            if (this.Selected?.IsSelectable == false)
            {
                this.ClearSelection();
            }

            this.GetSelectable();

            if (this.SecondaryButton.WasReleased)
            {
                this.ClearSelection();
            }
            else if (this.PrimaryButton.WasPressed)
            {
                if (this.Hover == null)
                {
                    return;
                }

                this.ClearSelection();

                this.Selected = this.Hover;
                this.Selected.SetSelected(true);
                this.OnSelection?.Invoke(new SelectionEvent(this.Hover));
            }
        }

        private void ClearSelection()
        {
            this.Selected?.SetSelected(false);
            this.Selected = null;
        }

        private void GetSelectable()
        {
            var hitColliders = Physics2D.OverlapCircleAll(this.GameData.GameCameraData.WorldPosition, .05f, this.GameData.BuildingLayer);

            float bestDistance = 0;
            ISelectable toSelect = null;

            foreach (var collider in hitColliders)
            {
                ISelectable selectable = collider.transform.GetProxyComponent<ISelectable>();

                if (selectable == null)
                    continue;

                float distance = collider.ClosestPoint(this.GameData.GameCameraData.WorldPosition).sqrMagnitude;

                if (toSelect == null || distance < bestDistance)
                {
                    toSelect = selectable;
                    bestDistance = distance;
                }
            }

            this.Hover = toSelect;
        }
    }
}