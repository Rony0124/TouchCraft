using Cinemachine.Utility;
using PlayerInput;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float velocity = 1f;

        private Transform cameraTransform;
        private CharacterController characterController;
        private bool isMoving = false;
    
        public bool IsOngoing => isMoving;
    
        private void Awake() {
            characterController = GetComponent<CharacterController>();
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            SetPlayerRotation();
            Move();
        }

        private void Move()
        {
            var axis = PlayerInputHandler.Instance.localInputInfo.InputAxis;
            if (axis != Vector2.zero)
            {
                var direction = new Vector3(axis.x, 0, axis.y);
                direction = cameraTransform.forward * direction.z + cameraTransform.right * direction.x;
                direction.y = 0;
                
                characterController.Move(direction.normalized * (velocity * Time.deltaTime));
                
                isMoving = true;
                return;
            }

            Cancel();
        }
        
        private void SetPlayerRotation()
        {
       
        }

        private void Cancel() {
            isMoving = false;
        }
    }
}
