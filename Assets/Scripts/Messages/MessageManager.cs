using Simpolony.Misc;
using Simpolony.UI.MessagePanelElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Simpolony.Messages
{
    [CreateAssetMenu(fileName = "Message Manager", menuName = "Data/Messages/new Message Manager")]
    public class MessageManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        [field: SerializeField, Header("Templates")] private VisualTreeAsset MessageTemplate { get; set; }

        List<Message> Messages { get; set; }

        private VisualElement MessagePanelElement { get; set; }



        public override void DoAwake()
        {
            Debug.Log("DoAwake Manager");
            this.Messages = new List<Message>();
            this.MessagePanelElement = null;
        }

        public override void DoStart()
        {

        }

        public override void DoUpdate()
        {
            for (int i = this.Messages.Count - 1; i >= 0; i--)
            {
                Message message = this.Messages[i];

                if (message.IsExpired)
                {
                    message.Destroy();
                    this.Messages.RemoveAt(i);
                    continue;
                }

                message.UpdateOpacity();
            }
        }

        public Message AddMessage(string content) => this.AddMessage(content, 4);
        public Message AddMessage(string content, float duration)
        {
            TemplateContainer template = this.MessageTemplate.Instantiate();

            Message message = new Message(Time.time + duration);
            message.Initialize(null, template);

            string finalMessage = $"{TimeSpan.FromSeconds(this.GameData.WaveManager.TotalWaveTime).ToString(@"mm\:ss")} : {content}";

            message.SetText(finalMessage);

            this.Messages.Add(message);

            // Add to Visual UI
            this.MessagePanelElement.Add(template);

            return message;
        }

        internal void SetPanel(MessagePanel messagePanel)
        {
            this.MessagePanelElement = messagePanel.MessagePanelElement;
        }
    }
}