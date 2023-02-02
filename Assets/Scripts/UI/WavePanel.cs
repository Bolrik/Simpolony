using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.UI
{
    [System.Serializable]
    public class WavePanel : UIComponent
    {
        [field: SerializeField, Header("Names")] private string WaveDisplayName { get; set; } = "WaveDisplay";
        [field: SerializeField] private string WaveFillName { get; set; } = "Fill";
        [field: SerializeField] private string WaveNumberName { get; set; } = "WaveNumber";
        [field: SerializeField] private string EnemyCountName { get; set; } = "EnemyCount";
        [field: SerializeField] private string WaveTimeName { get; set; } = "WaveTime";
        [field: SerializeField] private string ScoreName { get; set; } = "Score";


        [field: SerializeField] private GameData GameData { get; set; }

        VisualElement WaveDisplay { get; set; }
        VisualElement Fill { get; set; }

        Label WaveNumber { get; set; }
        Label EnemyCount { get; set; }
        Label WaveTime { get; set; }
        Label Score { get; set; }


        internal override void Initialize(UIElement root, VisualElement visualElement)
        {
            this.WaveDisplay = visualElement.Q<VisualElement>(this.WaveDisplayName);
            this.Fill = this.WaveDisplay.Q<VisualElement>(this.WaveFillName);

            this.WaveNumber = this.WaveDisplay.Q<Label>(this.WaveNumberName);
            this.EnemyCount = this.WaveDisplay.Q<Label>(this.EnemyCountName);
            this.WaveTime = this.WaveDisplay.Q<Label>(this.WaveTimeName);
            this.Score = this.WaveDisplay.Q<Label>(this.ScoreName);

            root.OnUpdate += this.Update;
        }

        private void Update()
        {
            this.WaveNumber.text = $"{this.GameData.WaveManager.WaveNumber}";
            this.EnemyCount.text = $"{this.GameData.WaveManager.EnemiesRemaining}";
            this.WaveTime.text = $"{TimeSpan.FromSeconds(this.GameData.WaveManager.TotalWaveTime).ToString(@"mm\:ss")}";
            this.Score.text = $"{this.GameData.ScoreManager.Score}";
            
            this.Fill.style.right = new StyleLength(new Length((this.GameData.WaveManager.WaveTimer / this.GameData.WaveManager.WaveTimerMax) * 100, LengthUnit.Percent));
        }

        public void SetVisible(bool state)
        {
            this.WaveDisplay.style.visibility = state ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
