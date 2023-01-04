using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Simpolony
{
    public class InputManager : FreschGames.Core.Input.InputManagerBase
    {
        protected override IInputActionCollection2 GetInputAssetObject()
        {
            return new GameInput();
        }
    }
}