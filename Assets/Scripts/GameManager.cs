using FreschGames.Core.Input;
using Simpolony.Buildings;
using Simpolony.State;
using Simpolony.UI.MessagePanelElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simpolony
{
    public class GameManager : MonoBehaviour
    {
        public static bool PlayTutorial { get; set; } = true;

        [field: SerializeField, Header("Data")] private GameData Data { get; set; }

        [field: SerializeField, Header("Tutorial")] private TutorialMessageContent[] TutorialMessages { get; set; }


        [field: SerializeField] private GameManagerProxy GameManagerProxy { get; set; }

        bool IsTutorial { get; set; }
        int TutorialIndex { get; set; } = -1;

        Message TutorialMessage { get; set; }

        Building Core { get; set; }

        bool IsGameOver { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            this.Data.GameStateManager.SetState(GameState.Idle);

            this.IsTutorial = PlayTutorial;
            PlayTutorial = false;

            if (this.IsTutorial)
                this.Data.ResourceManager.SetResources(0);
            else
                this.Data.ResourceManager.SetResources(10);
            
            this.GetCore();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.IsGameOver)
                return;

            this.UpdateTutorial();
            this.CheckGameOver();


            // Don't know where to put this =D
            if (this.Data.SelectionManager.Selected is Building building &&
                this.Data.Input.DestroyButton.WasPressed)
            {
                building.Destroy();
            }
        }


        private void CheckGameOver()
        {
            this.GetCore();

            if (this.Core == null || !this.Core.Health.IsAlive)
            {
                this.GameOver();
            }
        }

        private void GetCore()
        {
            if (this.Core != null)
                return;

            foreach (var building in this.Data.BuildingManager.Buildings)
            {
                if (building.Value.Data.DataType == BuildingData.BuildingDataType.Core)
                {
                    this.Core = building.Value;
                }
            }
        }

        private void GameOver()
        {
            this.IsGameOver = true;
            this.Data.WaveManager.Pause();

            this.Data.MessageManager.AddMessage("Attention survivors, the fate of your colony has been sealed. The relentless enemy has breached your defenses and overrun your colony. The skies are filled with smoke, the ground is littered with debris, and the silence is only broken by the screams of the fallen. We cannot express the sadness we feel for the loss of your colony, but we remind you that in this harsh world, failure is not the end. Take this experience as a learning opportunity and rise from the ashes.", 60);
        }


        private void UpdateTutorial()
        {
            if (!this.IsTutorial)
                return;

            this.NextTutorialMessage();
        }

        private void NextTutorialMessage()
        {
            if (this.TutorialIndex >= 0)
            {
                TutorialMessageContent current = this.TutorialMessages[this.TutorialIndex];

                if (!this.IsTutorialMessageDone(current))
                {
                    if (this.TutorialMessage?.IsExpired == true)
                        this.ShowMessage(current);
                    return;
                }
            }

            this.TutorialMessage = null;
            
            this.TutorialIndex++;

            if (this.TutorialIndex >= this.TutorialMessages.Length)
            {
                this.IsTutorial = false;
                return;
            }

            TutorialMessageContent next = this.TutorialMessages[this.TutorialIndex];

            if (next.RepeatUntil != TutorialMessageCondition.None && this.IsTutorialMessageDone(next))
                return;

            this.ShowMessage(next);
        }

        private bool IsTutorialMessageDone(TutorialMessageContent messageContent)
        {
            if (this.TutorialMessage?.IsExpired == false)
                return false;

            switch (messageContent.RepeatUntil)
            {
                case TutorialMessageCondition.CheckMine:
                    {
                        foreach (var building in this.Data.BuildingManager.Buildings)
                        {
                            if (building.Value.Data.DataType == BuildingData.BuildingDataType.Mine)
                                return true;
                        }
                    }
                    break;
                case TutorialMessageCondition.CheckTower:
                    {
                        foreach (var building in this.Data.BuildingManager.Buildings)
                        {
                            if (building.Value.Data.DataType == BuildingData.BuildingDataType.Tower)
                                return true;
                        }
                    }
                    break;
                case TutorialMessageCondition.CheckStart:
                    {
                        return this.Data.BuildingManager.Buildings.Count >= 6;
                    }
                case TutorialMessageCondition.None:
                    return true;
                default:
                    break;
            }

            return false;
        }

        private void ShowMessage(TutorialMessageContent next)
        {
            this.TutorialMessage = this.Data.MessageManager.AddMessage(next.Message, next.Duration);
            this.RunCommand(next);
        }

        private void RunCommand(TutorialMessageContent messageContent)
        {
            switch (messageContent.OnStart)
            {
                case TutorialMessageCommand.None:
                    break;
                case TutorialMessageCommand.StopWave:
                    this.GameManagerProxy.GameScreen.WavePanel.SetVisible(false);
                    this.Data.WaveManager.Pause();
                    break;
                case TutorialMessageCommand.StartWave:
                    this.GameManagerProxy.GameScreen.WavePanel.SetVisible(true);
                    this.Data.WaveManager.Resume();
                    break;
                case TutorialMessageCommand.ResMine:
                    if (this.Data.ResourceManager.Available < 3)
                        this.Data.ResourceManager.SetResources(3);
                    break;
                case TutorialMessageCommand.ResTower:
                    if (this.Data.ResourceManager.Available < 7)
                        this.Data.ResourceManager.SetResources(7);
                    break;
                default:
                    break;
            }
        }

        [System.Serializable]
        class TutorialMessageContent
        {
            [field: SerializeField] public TutorialMessageCondition RepeatUntil { get; private set; }
            [field: SerializeField] public TutorialMessageCommand OnStart { get; private set; }
            [field: SerializeField] public string Message { get; private set; }
            [field: SerializeField] public int Duration { get; private set; }
        }

        enum TutorialMessageCommand
        {
            None,
            StopWave,
            StartWave,
            ResMine,
            ResTower
        }

        enum TutorialMessageCondition
        {
            None,
            CheckMine,
            CheckTower,
            CheckStart
        }
    }
}