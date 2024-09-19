using Input;
using UnityEngine;
using Util;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
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

        private void Move()
        {
            var direction = PlayerInputHandler.Instance.localInputInfo.InputAxis;
            if (direction != Vector2.zero)
            {
                playerMovement.Execute(direction);
                return;
            }

            playerMovement.Cancel();
        }
    }
}
