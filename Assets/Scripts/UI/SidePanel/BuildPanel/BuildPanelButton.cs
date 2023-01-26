using Simpolony.Buildings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI.SidePanelElements.BuildPanelElements
{
    [System.Serializable]
    public class BuildPanelButton : UIComponent
    {
        [field: SerializeField] private GameData GameData { get; set; }
        [field: SerializeField] private BuildingData BuildingData { get; set; }

        private string ButtonName { get; set; } = "BuildMenuButton";
        private string TitleName { get; set; } = "Title";
        private string ImageName { get; set; } = "Background";
        private string CostName { get; set; } = "Cost";

        Button Button { get; set; }
        Label Title { get; set; }
        Label Cost { get; set; }
        VisualElement Image { get; set; }

        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.Button = visualElement.Q<Button>(this.ButtonName);
            this.Button.clicked += this.OnClick;

            this.Title = this.Button.Q<Label>(this.TitleName);
            this.Cost = this.Button.Q<Label>(this.CostName);
            this.Image = this.Button.Q<VisualElement>(this.ImageName);

            this.Title.text = $"{this.BuildingData.name}"; //this.DisplayName;
            this.Cost.text = $"{this.BuildingData.ResourceCost}";
            this.Image.style.backgroundImage = new StyleBackground(this.BuildingData.MenuSprite);
        }

        private void OnClick()
        {
            this.GameData.BuildingConstructionManager.StartPreview(this.BuildingData);
        }
    }
}
