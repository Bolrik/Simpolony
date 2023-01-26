using Simpolony.UI.SidePanelElements.BuildPanelElements;
using Simpolony.UI.SidePanelElements.InfoPanelElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.SidePanelElements
{
    [System.Serializable]
    public class SidePanel : UIComponent
    {
        [field: SerializeField, Header("Names")] public string SidePanelName { get; private set; } = "SidePanel";

        [field: SerializeField, Header("Components")] private BuildPanel BuildPanel { get; set; }
        [field: SerializeField] private InfoPanel InfoPanel { get; set; }
        
        
        VisualElement SidePanelElement { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.SidePanelElement = visualElement.Q<VisualElement>(this.SidePanelName);

            this.BuildPanel.Initialize(root, this.SidePanelElement);
            this.InfoPanel.Initialize(root, this.SidePanelElement);
        }
    }
}