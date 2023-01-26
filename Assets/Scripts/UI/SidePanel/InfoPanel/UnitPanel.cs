using Simpolony.Resources;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.SidePanelElements.InfoPanelElements.ResourcePanelElements.UnitPanelElements
{
    [System.Serializable]
    public class UnitPanel : UIComponent
    {
        [field: SerializeField, Header("Names")] private string UnitPanelName { get; set; } = "UnitPanel";
        [field: SerializeField] private string CurrentElementName { get; set; } = "Current";
        [field: SerializeField] private string TotalElementName { get; set; } = "Total";


        [field: SerializeField] private GameData GameData { get; set; }
        private UnitManager UnitManager { get => this.GameData.UnitManager; }

        private VisualElement UnitPanelElement { get; set; }
        private Label CurrentElement { get; set; }
        private Label TotalElement { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.UnitPanelElement = visualElement.Q<VisualElement>(this.UnitPanelName);

            this.CurrentElement = this.UnitPanelElement.Q<Label>(this.CurrentElementName);
            this.TotalElement = this.UnitPanelElement.Q<Label>(this.TotalElementName);

            root.OnUpdate += this.Update;
        }

        private void Update()
        {
            this.CurrentElement.text = $"{this.UnitManager.Current}";
            this.TotalElement.text = $"{this.UnitManager.Capacity}";
        }
    }
}
