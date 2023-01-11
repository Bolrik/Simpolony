using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI
{
    public class GameScreen : UIElement
    {
        [field: SerializeField] private GameScreenBuildMenu GameScreenBuildMenu { get; set; }

        private void Start()
        {
            this.GameScreenBuildMenu.Initialize(this, this.Document.rootVisualElement);
        }
    }

    [System.Serializable]
    public class GameScreenBuildMenu
    {
        [field: SerializeField] private string Name { get; set; } = "BuildMenu";

        [field: SerializeField] private VisualTreeAsset ButtonTemplate { get; set; }

        VisualElement MenuRoot { get; set; }
        List<GameScreenBuildMenuButton> Buttons { get; set; } = new List<GameScreenBuildMenuButton>();

        internal void Initialize(UIElement root, VisualElement visualElement)
        {
            this.MenuRoot = visualElement.Q<VisualElement>(this.Name);


            for (int i = 0; i < 5; i++)
            {
                TemplateContainer template = this.ButtonTemplate.Instantiate();
                this.MenuRoot.Add(template);

                GameScreenBuildMenuButton button = new GameScreenBuildMenuButton();

                this.Buttons.Add(button);
                button.Initialize(root, template);
            }
        }
    }

    [System.Serializable]
    public class GameScreenBuildMenuButton
    {
        [field: SerializeField] private string Name { get; set; } = "BuildMenuButton";

        Button Button { get; set; }

        internal void Initialize(UIElement root, VisualElement visualElement)
        {
            this.Button = visualElement.Q<Button>(this.Name);
            this.Button.style.backgroundColor = new Color(Random.value, Random.value, Random.value);
        }
    }
}
