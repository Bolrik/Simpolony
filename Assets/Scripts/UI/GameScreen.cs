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
        [field: SerializeField] private SidePanel SidePanel { get; set; }
        [field: SerializeField] private WavePanel WavePanel { get; set; }

        private void Start()
        {
            this.SidePanel.Initialize(this, this.Document.rootVisualElement);
            this.WavePanel.Initialize(this, this.Document.rootVisualElement);
        }
    }



    [System.Serializable]
    public class WavePanel
    {
        [field: SerializeField, Header("Names")] private string WaveDisplayName { get; set; } = "WaveDisplay";
        [field: SerializeField] private string WaveNumberName { get; set; } = "WaveNumber";
        [field: SerializeField] private string WaveFillName { get; set; } = "Fill";
        [field: SerializeField] private string EnemyCountName { get; set; } = "EnemyCount";


        [field: SerializeField] private GameData GameData { get; set; }

        VisualElement WaveDisplay { get; set; }
        Label WaveNumber { get; set; }
        Label EnemyCount { get; set; }
        VisualElement Fill { get; set; }


        internal void Initialize(UIElement root, VisualElement visualElement)
        {
            this.WaveDisplay = visualElement.Q<VisualElement>(this.WaveDisplayName);
            this.WaveNumber = this.WaveDisplay.Q<Label>(this.WaveNumberName);
            this.EnemyCount = this.WaveDisplay.Q<Label>(this.EnemyCountName);
            this.Fill = this.WaveDisplay.Q<VisualElement>(this.WaveFillName);

            root.OnUpdate += this.Update;
        }

        private void Update()
        {
            this.WaveNumber.text = $"{this.GameData.WaveManager.WaveNumber}";
            this.EnemyCount.text = $"{this.GameData.WaveManager.EnemiesRemaining}";
            
            this.Fill.style.right = new StyleLength(new Length((this.GameData.WaveManager.WaveTimer / this.GameData.WaveManager.WaveTimerMax) * 100, LengthUnit.Percent));
        }
    }
}
