using Simpolony.Messages;
using Simpolony.UI.SidePanelElements;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.MessagePanelElements
{
    [System.Serializable]
    public class MessagePanel : UIComponent
    {
        [field: SerializeField, Header("Names")] private string MessagePanelName { get; set; } = "MessagePanel";
        
        public VisualElement MessagePanelElement { get; private set; }

        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.MessagePanelElement = visualElement.Q<VisualElement>(this.MessagePanelName);
        }
    }
}