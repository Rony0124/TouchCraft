using Character;
using Cinemachine.Utility;
using PlayerInput;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float defaultVelocity = 7f;
        [SerializeField] private float crouchVelocity = 3f;
        [SerializeField] private float runVelocity = 11f;
        
        private float appliedVelocity;
        private Transform cameraTransform;
        private CharacterController characterController;
        private CharacterAnimator characterAnimator;
        
        public bool IsMoving { get;set; }
        
        public bool IsRunning { get; set; }
        public bool IsCrouching { get; set; }
        
        private void Awake() {
            characterController = GetComponent<CharacterController>();
            characterAnimator = GetComponentInChildren<CharacterAnimator>();
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            SetPlayerRotation();
            Move();
            
            characterAnimator.MovementUpdated(this);
        }

        private void Move()
        {
            var axis = PlayerInputHandler.Instance.localInputInfo.InputAxis;
            IsRunning = PlayerInputHandler.Instance.localInputInfo.IsRunning;
            IsCrouching = PlayerInputHandler.Instance.localInputInfo.IsCrouching;
                
            if (axis != Vector2.zero)
            {
                var direction = new Vector3(axis.x, 0, axis.y);
                direction = cameraTransform.forward * direction.z + cameraTransform.right * direction.x;
                direction.y = 0;

                appliedVelocity = IsRunning ? runVelocity : defaultVelocity;
                if (IsCrouching)
                {
                    appliedVelocity = crouchVelocity;
                }
                
                characterController.Move(direction.normalized * (appliedVelocity * Time.deltaTime));
                
                IsMoving = true;
                return;
            }

            Cancel();
        }
        
        private void SetPlayerRotation()
        {
            var eulerAngles = transform.localRotation.eulerAngles;
            eulerAngles.y = cameraTransform.eulerAngles.y;
            transform.localRotation = Quaternion.Euler(eulerAngles);
        }

        private void Cancel() {
            IsMoving = false;
        }
    }
}
