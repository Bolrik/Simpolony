using Simpolony.UI.SidePanelElements.InfoPanelElements.ResourcePanelElements;
using Simpolony.UI.SidePanelElements.InfoPanelElements.ResourcePanelElements.UnitPanelElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.SidePanelElements.InfoPanelElements
{
    [System.Serializable]
    public class InfoPanel : UIComponent
    {
        [field: SerializeField, Header("Names")] public string InfoPanelName { get; private set; } = "InfoPanel";
        
        [field: SerializeField, Header("Components")] public ResourcePanel ResourcePanel { get; private set; }
        [field: SerializeField] public UnitPanel UnitPanel { get; private set; }
        

        private VisualElement InfoPanelElement { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.InfoPanelElement = visualElement.Q<VisualElement>(this.InfoPanelName);

            this.ResourcePanel.Initialize(root, this.InfoPanelElement);
            this.UnitPanel.Initialize(root, this.InfoPanelElement);
        }
    }
}
