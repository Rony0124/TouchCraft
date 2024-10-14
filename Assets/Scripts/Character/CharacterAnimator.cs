using Player;
using UnityEngine;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
        private static readonly int IsCrouchingHash = Animator.StringToHash("IsCrouching");
        
        private Animator _animator;
     
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void MovementUpdated(CharacterMovement movement)
        {
            _animator.SetBool(IsCrouchingHash, movement.IsCrouching);
            _animator.SetBool(IsRunningHash, movement.IsRunning);
            _animator.SetBool(IsMovingHash, movement.IsMoving);
        }
    }
}
