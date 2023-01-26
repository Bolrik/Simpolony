using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.SidePanelElements.BuildPanelElements
{
    [System.Serializable]
    public class BuildPanel : UIComponent
    {
        [field: SerializeField, Header("Names")] public string BuildPanelName { get; private set; } = "BuildPanel";

        [field: SerializeField, Header("Templates")] private VisualTreeAsset ButtonTemplate { get; set; }
        
        [field: SerializeField, Header("Settings")] private BuildPanelButton[] BuildMenuButtons { get; set; }

        private VisualElement BuildPanelElement { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.BuildPanelElement = visualElement.Q<VisualElement>(this.BuildPanelName);

            for (int i = 0; i < this.BuildMenuButtons.Length; i++)
            {
                TemplateContainer template = this.ButtonTemplate.Instantiate();

                BuildPanelButton button = this.BuildMenuButtons[i];
                button.Initialize(root, template);

                // Add to Visual UI
                this.BuildPanelElement.Add(template);

                //this.Buttons.Add(button);
            }
        }
    }
}
