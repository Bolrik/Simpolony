using Simpolony.Buildings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI
{
    [System.Serializable]
    public class ButtonPanel : UIComponent
    {
        [field: SerializeField, Header("Names")] private string ButtonPanelName { get; set; } = "ButtonPanel";
        [field: SerializeField] private string RemoveBuildingName { get; set; } = "RemoveBuilding";

        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        VisualElement ButtonPanelElement { get; set; }

        Button RemoveBuildingElement { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.ButtonPanelElement = visualElement.Q<VisualElement>(this.ButtonPanelName);
            this.RemoveBuildingElement = this.ButtonPanelElement.Q<Button>(this.RemoveBuildingName);

            this.RemoveBuildingElement.clicked += this.RemoveButtonElement_Clicked;

            root.OnUpdate += this.Update;
        }

        private void RemoveButtonElement_Clicked()
        {
            if (!(this.GameData.SelectionManager.Selected is Building building))
                return;

            building.Destroy();
        }

        private void Update()
        {
            bool state = this.GameData.SelectionManager.Selected is Building;
            this.RemoveBuildingElement.SetEnabled(state);
        }
    }
}
