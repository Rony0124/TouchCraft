using Cinemachine.Utility;
using PlayerInput;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float velocity = 1f;
        [SerializeField] private float rotateSpeed = 1f;
    
        [SerializeField] private Vector3 verticalAxis;
        [SerializeField] private Vector3 horizontalAxis;
    
        private CharacterController characterController;
        private bool isMoving = false;
    
        private Vector3 upAxis => Vector3.Cross(verticalAxis, horizontalAxis).normalized;
    
        public bool IsOngoing => isMoving;
    
        private void Awake() {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var axis = PlayerInputHandler.Instance.localInputInfo.InputAxis;
            if (axis != Vector2.zero)
            {
                var direction = Vector3.right * axis.x + Vector3.forward * axis.y;
            
                Execute(direction);
                return;
            }

            Cancel();
        }

        private void Execute(Vector3 direction) {
            SetPlayerRotation(direction);
            characterController.Move(transform.forward * (velocity * Time.deltaTime));
        
            isMoving = true;
        }
    
        void SetPlayerRotation(Vector3 dir)
        {
            var orientation = dir;
            orientation.y = 0.0f;

            if (orientation.AlmostZero())
                return;
        
            var tr = transform;
            var normalizedOrientation = orientation.normalized;

            var current = tr.rotation;
            var target = Quaternion.LookRotation(normalizedOrientation, upAxis);

            tr.rotation = Quaternion.RotateTowards(current, target, rotateSpeed * Time.deltaTime);
        }

        private void Cancel() {
            isMoving = false;
        }
    }
}
