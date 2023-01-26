using Simpolony.Resources;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.SidePanelElements.InfoPanelElements.ResourcePanelElements
{
    [System.Serializable]
    public class ResourcePanel : UIComponent
    {
        [field: SerializeField, Header("Names")] private string ResourcePanelName { get; set; } = "ResourcePanel";
        [field: SerializeField] private string ValueElementName { get; set; } = "Value";

        [field: SerializeField, Header("References")] public ResourceManager ResourceManager { get; private set; }

        private VisualElement ResourcePanelElement { get; set; }
        private Label ValueElement { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.ResourcePanelElement = visualElement.Q<VisualElement>(this.ResourcePanelName);

            this.ValueElement = this.ResourcePanelElement.Q<Label>(this.ValueElementName);

            root.OnUpdate += this.Update;
        }

        private void Update()
        {
            this.ValueElement.text = $"{this.ResourceManager.Available}";
        }
    }
}
