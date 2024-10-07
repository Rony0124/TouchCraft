using Player;
using UnityEngine;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        
        private Animator _animator;
     
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void MovementUpdated(CharacterMovement movement)
        {
            _animator.SetBool(IsMovingHash, movement.IsMoving);
        }
    }
}
