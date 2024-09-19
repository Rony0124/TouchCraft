using System;
using Input;
using UnityEngine;
using Util;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            PlayerInputHandler.Instance.OnInputTriggered += OnInputAction;
        }

        private void OnInputAction(PlayerInputKey playerInputKey)
        {
            switch (playerInputKey)
            {
                case PlayerInputKey.Move:
                case PlayerInputKey.None:
                default:
                    break;
            }
        }

    
    }
}
