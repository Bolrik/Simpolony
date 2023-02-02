using Simpolony.UI.SidePanelElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Simpolony.UI
{
    [System.Serializable]
    public class MenuPanel : UIComponent
    {
        [field: SerializeField, Header("Names")] public string MenuPanelName { get; private set; } = "MenuPanel";
        [field: SerializeField] public string ResumeButtonName { get; private set; } = "Resume";
        [field: SerializeField] public string RestartButtonName { get; private set; } = "Restart";
        [field: SerializeField] public string TutorialRestartButtonName { get; private set; } = "RestartTutorial";
        [field: SerializeField] public string QuitButtonName { get; private set; } = "Quit";
        [field: SerializeField] public string MouseSensitivityName { get; private set; } = "MouseSensitivity";
        [field: SerializeField] public string ScreenShakeName { get; private set; } = "ScreenShake";


        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        VisualElement MenuPanelElement { get; set; }

        Button ResumeButtonElement { get; set; }
        Button RestartButtonElement { get; set; }
        Button TutorialRestartButtonElement { get; set; }
        Button QuitButtonElement { get; set; }
        Slider MouseSensitivityElement { get; set; }
        Toggle ScreenShakeElement { get; set; }

        bool IsActive { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.MenuPanelElement = visualElement.Q<VisualElement>(this.MenuPanelName);

            this.ResumeButtonElement = this.MenuPanelElement.Q<Button>(this.ResumeButtonName);
            this.RestartButtonElement = this.MenuPanelElement.Q<Button>(this.RestartButtonName);
            this.TutorialRestartButtonElement = this.MenuPanelElement.Q<Button>(this.TutorialRestartButtonName);
            this.QuitButtonElement = this.MenuPanelElement.Q<Button>(this.QuitButtonName);


            this.ResumeButtonElement.clicked += this.ResumeButtonElement_Clicked;
            this.RestartButtonElement.clicked += this.RestartButtonElement_Clicked;
            this.TutorialRestartButtonElement.clicked += this.TutorialRestartButtonElement_Clicked;
            this.QuitButtonElement.clicked += this.QuitButtonElement_Clicked;

            this.MouseSensitivityElement = this.MenuPanelElement.Q<Slider>(this.MouseSensitivityName);
            this.MouseSensitivityElement.value = this.GameData.GameCameraData.MouseCameraSpeed;

            this.ScreenShakeElement = this.MenuPanelElement.Q<Toggle>(this.ScreenShakeName);
            this.ScreenShakeElement.value = this.GameData.GameCameraData.IsScreenShakeActive;

            root.OnUpdate += this.Update;
        }

        private void Update()
        {
            this.GameData.GameCameraData.MouseCameraSpeed = this.MouseSensitivityElement.value;
            this.GameData.GameCameraData.IsScreenShakeActive = this.ScreenShakeElement.value;
        }

        public void Toggle()
        {
            this.IsActive = !this.IsActive;

            this.MenuPanelElement.style.visibility = this.IsActive ? Visibility.Visible : Visibility.Hidden;
        }

        private void ResumeButtonElement_Clicked()
        {
            Debug.Log("Resume");
            this.MenuPanelElement.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
        }

        private void RestartButtonElement_Clicked()
        {
            Debug.Log("Restart");
            SceneManager.LoadScene(0);
        }

        private void TutorialRestartButtonElement_Clicked()
        {
            Debug.Log("Tutorial Restart");
            GameManager.PlayTutorial = true;
            SceneManager.LoadScene(0);
        }

        private void QuitButtonElement_Clicked()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}