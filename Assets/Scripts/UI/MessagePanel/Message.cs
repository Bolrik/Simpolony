using Simpolony.UI;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.MessagePanelElements
{
    [System.Serializable]
    public class Message : UIComponent
    {
        private string MessageName { get; set; } = "Message";
        private string TextName { get; set; } = "Text";

        VisualElement MessageElement { get; set; }
        Label TextElement { get; set; }

        float TerminationTime { get; set; }
        public bool IsExpired { get => this.TerminationTime <= Time.time; }

        public Message(float terminationTimer)
        {
            this.TerminationTime = terminationTimer;
        }

        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.MessageElement = visualElement.Q<VisualElement>(this.MessageName);

            this.TextElement = this.MessageElement.Q<Label>(this.TextName);
        }

        public void SetText(string text)
        {
            this.TextElement.text = text;
        }

        public void UpdateOpacity()
        {
            float delta = (this.TerminationTime - Time.time).ClampMax(1);
            this.MessageElement.style.opacity = delta;
        }

        public void Destroy()
        {
            this.MessageElement.RemoveFromHierarchy();
        }
    }
}