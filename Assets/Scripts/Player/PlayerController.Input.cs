using PlayerInput;
using Util;

namespace Player
{
    public partial class PlayerController
    {
        private void InitInput()
        {
            PlayerInputHandler.Instance.OnInputTriggered += OnInputAction;
        }
        
        private void DisableInput()
        {
            PlayerInputHandler.Instance.OnInputTriggered -= OnInputAction;
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
