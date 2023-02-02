using Simpolony.UI.MessagePanelElements;
using Simpolony.UI.SidePanelElements;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI
{
    public class GameScreen : UIElement
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        [field: SerializeField, Header("Components")] public SidePanel SidePanel { get; private set; }
        [field: SerializeField] public ButtonPanel ButtonPanel { get; private set; }
        [field: SerializeField] public WavePanel WavePanel { get; private set; }
        [field: SerializeField] public MenuPanel MenuPanel { get; private set; }
        [field: SerializeField] public MessagePanel MessagePanel { get; private set; }
        [field: SerializeField] public HelpPanel HelpPanel { get; private set; }

        private void Awake()
        {
            this.SidePanel.Initialize(this, this.Document.rootVisualElement);
            this.ButtonPanel.Initialize(this, this.Document.rootVisualElement);
            this.WavePanel.Initialize(this, this.Document.rootVisualElement);
            this.MenuPanel.Initialize(this, this.Document.rootVisualElement);
            this.MessagePanel.Initialize(this, this.Document.rootVisualElement);
            this.HelpPanel.Initialize(this, this.Document.rootVisualElement);
        }

        protected override void Update()
        {
            base.Update();

            if (this.GameData.Input.MenuButton.WasPressed)
            {
                this.MenuPanel.Toggle();
            }
        }
    }

    [System.Serializable]
    public class HelpPanel : UIComponent
    {
        [field: SerializeField, Header("Names")] public string HelpPanelName { get; private set; } = "HelpPanel";
        [field: SerializeField] private string ExitButtonName { get; set; } = "ExitButton";

        VisualElement HelpPanelElement { get; set; }
        Button ExitButtonElement { get; set; }

        bool IsActive { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.HelpPanelElement = visualElement.Q<VisualElement>(this.HelpPanelName);

            this.ExitButtonElement = this.HelpPanelElement.Q<Button>(this.ExitButtonName);


            this.ExitButtonElement.clicked += this.ExitButtonElement_Clicked;
        }

        private void ExitButtonElement_Clicked()
        {
            this.Toggle();
        }

        public void Toggle()
        {
            this.IsActive = !this.IsActive;
            this.HelpPanelElement.style.visibility = this.IsActive ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
