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
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var camAxis = context.ReadValue<Vector2>();
            _handler.localInputInfo.CamAxis = camAxis;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnInputTriggered?.Invoke(PlayerInputKey.Interact);
        }
        
        public void OnRun(InputAction.CallbackContext context)
        {
            _handler.localInputInfo.IsRunning = context.action.IsInProgress();
        }
        
        public void OnCrouch(InputAction.CallbackContext context)
        {
            _handler.localInputInfo.IsCrouching = context.action.IsInProgress();
        }
    }
}
