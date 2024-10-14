using System;
using Character;
using PlayerInput;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class GravitySettings
    {
        public float Gravity = 20.0f; // Gravity applied when the player is airborne
        public float GroundedGravity = 5.0f; // A constant gravity that is applied when the player is grounded
        public float MaxFallSpeed = 40.0f; // The max speed at which the player can fall
    }
    
    [Serializable]
    public class GroundSettings
    {
        public LayerMask GroundLayers; // Which layers are considered as ground
        public float SphereCastRadius = 0.35f; // The radius of the sphere cast for the grounded check
        public float SphereCastDistance = 0.15f; // The distance below the character's capsule used for the sphere cast grounded check
    }
    
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float defaultVelocity = 7f;
        [SerializeField] private float crouchVelocity = 3f;
        [SerializeField] private float runVelocity = 11f;
        
        public GravitySettings GravitySettings;
        public GroundSettings GroundSettings;
        
        private float appliedVelocity;
        private float verticalSpeed;
        private Transform cameraTransform;
        
        private bool isGrounded;
        
        private CharacterController characterController;
        private CharacterAnimator characterAnimator;
        
        public bool IsMoving { get;set; }
        public bool IsRunning { get; set; }
        public bool IsCrouching { get; set; }
        public bool IsGrounded => isGrounded;
        
        private void Awake() {
            characterController = GetComponent<CharacterController>();
            characterAnimator = GetComponentInChildren<CharacterAnimator>();
            if (Camera.main != null) 
                cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            UpdateGrounded();
            
            SetPlayerRotation();
            Move();
            
            characterAnimator.MovementUpdated(this);
        }

        private void Move()
        {
            var axis = PlayerInputHandler.Instance.localInputInfo.InputAxis;
            IsRunning = PlayerInputHandler.Instance.localInputInfo.IsRunning;
            IsCrouching = PlayerInputHandler.Instance.localInputInfo.IsCrouching;

            if (isGrounded)
            {
                verticalSpeed = -GravitySettings.GroundedGravity;
            }
            else
            {
                verticalSpeed = Mathf.MoveTowards(verticalSpeed, -GravitySettings.MaxFallSpeed, GravitySettings.Gravity * Time.deltaTime);
            }
            
            Vector3 moveDir = new Vector3(axis.x, 0, axis.y);
            if (axis != Vector2.zero)
            {
                moveDir = cameraTransform.forward * moveDir.z + cameraTransform.right * moveDir.x;
                moveDir.y = 0;

                appliedVelocity = IsRunning ? runVelocity : defaultVelocity;
                if (IsCrouching)
                {
                    appliedVelocity = crouchVelocity;
                }
                
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }

            var horizontalMove = moveDir.normalized * appliedVelocity;
            var verticalMove = verticalSpeed * Vector3.up;
            characterController.Move((horizontalMove + verticalMove) * Time.deltaTime);
        }
        
        private void SetPlayerRotation()
        {
            var eulerAngles = transform.localRotation.eulerAngles;
            eulerAngles.y = cameraTransform.eulerAngles.y;
            transform.localRotation = Quaternion.Euler(eulerAngles);
        }
        
        private void UpdateGrounded()
        {
            isGrounded = CheckGrounded() && verticalSpeed <= 0.0f;
        }
        
        private bool CheckGrounded()
        {
            bool isGround = Physics.CheckSphere(
                CheckGroundSpherePosition(), GroundSettings.SphereCastRadius, GroundSettings.GroundLayers, QueryTriggerInteraction.Ignore);

            return isGrounded;
        }
        
        private Vector3 CheckGroundSpherePosition()
        {
            Vector3 spherePosition = transform.position;
            spherePosition.y = transform.position.y + GroundSettings.SphereCastRadius - GroundSettings.SphereCastDistance;
            return spherePosition;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(CheckGroundSpherePosition(), GroundSettings.SphereCastRadius);
        }
    }
}
