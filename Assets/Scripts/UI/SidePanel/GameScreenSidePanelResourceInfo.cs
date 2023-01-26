using Simpolony.Resources;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI
{
    [System.Serializable]
    public class GameScreenSidePanelResourceInfo
    {
        [field: SerializeField] private string ContainerName { get; set; } = "ResourceInfo";
        [field: SerializeField] private string ResourceLabelName { get; set; } = "Resources";


        [field: SerializeField] public ResourceManager ResourceManager { get; private set; }

        VisualElement Container { get; set; }
        Label ResourceLabel { get; set; }

        internal void Initialize(UIElement root, VisualElement visualElement)
        {
            this.Container = visualElement.Q<VisualElement>(this.ContainerName);
            this.ResourceLabel = this.Container.Q<Label>(this.ResourceLabelName);

            root.OnUpdate += this.Update;
        }

        private void Update()
        {
            this.ResourceLabel.text = $"{this.ResourceManager.Available}";
        }
    }
}
