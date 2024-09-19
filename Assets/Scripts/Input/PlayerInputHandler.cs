using System;
using Common;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Input
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerInputHandler", menuName = "SSO/PlayerInputHandler")]
    public class PlayerInputHandler : SingletonScriptableObject<PlayerInputHandler>
    {
        public struct InputInfo
        {
            public Vector2 InputAxis;
        }

        public InputInfo localInputInfo;
        
        private PlayerInputActions _playerInputActions;
        private PlayerInputCallback _playerInputCallback;
        
        private PlayerInputActions PlayerInputAction => _playerInputActions ?? new PlayerInputActions();
        public PlayerInputCallback PlayerInputCallback => _playerInputCallback ?? new PlayerInputCallback(this);
        
        public Action<PlayerInputKey> OnInputTriggered; 

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputCallback = new PlayerInputCallback(this);
            
            _playerInputActions.Player.SetCallbacks(_playerInputCallback);
        }

        private void OnEnable()
        {
            PlayerInputAction.Enable();
            PlayerInputCallback.OnInputTriggered += OnPlayerInputTriggered;
        }
        
        private void OnPlayerInputTriggered(PlayerInputKey playerInputKey)
        {
            OnInputTriggered?.Invoke(playerInputKey);
        }
    }
}
