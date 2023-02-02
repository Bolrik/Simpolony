using FreschGames.Core.Input;
using Simpolony.Misc;
using Simpolony.State;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class ConnectionSystem : SystemComponent<ConnectionManager>
    {
        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }
        ConnectionManager ConnectionManager { get => this.GameData.ConnectionManager; }

        protected override void OnAwake() { }

        protected override void OnStart() { }

        protected override void OnUpdate() { }

    }

}