using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace PlayerInput
{
    public class PlayerInputCallback : PlayerInputActions.IPlayerActions
    {
        public PlayerInputCallback(PlayerInputHandler handler)
        {
            _handler = handler;
        }

        private PlayerInputHandler _handler;
        
        public Action<PlayerInputKey> OnInputTriggered;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            var inputAxis = context.ReadValue<Vector2>();
            _handler.localInputInfo.InputAxis = inputAxis;
            
            OnInputTriggered?.Invoke(PlayerInputKey.Move);
        }
    }
}
