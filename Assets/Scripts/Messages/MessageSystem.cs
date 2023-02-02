using Simpolony.Misc;
using Simpolony.UI;
using Simpolony.UI.MessagePanelElements;
using UnityEngine;

namespace Simpolony.Messages
{
    public class MessageSystem : SystemComponent<MessageManager>
    {
        [field: SerializeField] private GameScreen GameScreen { get; set; }

        protected override void OnAwake()
        {
            Debug.Log("Set Panel System");
            this.Manager.SetPanel(this.GameScreen.MessagePanel);
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnUpdate()
        {
            
        }
    }
}