using Cinemachine.Utility;
using PlayerInput;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float velocity = 1f;
        //[SerializeField] private float rotateSpeed = 1f;
        [SerializeField] private float camFollowSpeed = 1f;
        
        [Header("Look Target")]
        [SerializeField] private Transform camFollowTargetTr;
        
        /*[Header("Rotation Angle")]
        [SerializeField] private Vector3 verticalAxis;
        [SerializeField] private Vector3 horizontalAxis;*/
    
        private CharacterController characterController;
        private bool isMoving = false;
    
       // private Vector3 upAxis => Vector3.Cross(verticalAxis, horizontalAxis).normalized;
    
        public bool IsOngoing => isMoving;
    
        private void Awake() {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Rotate();
            Move();
        }

        private void Move()
        {
            var axis = PlayerInputHandler.Instance.localInputInfo.InputAxis;
            if (axis != Vector2.zero)
            {
                var direction = transform.right * axis.x + transform.forward * axis.y;
                characterController.Move(direction.normalized * (velocity * Time.deltaTime));
                
                isMoving = true;
                return;
            }

            Cancel();
        }

        private void Rotate()
        {
            var axis = PlayerInputHandler.Instance.localInputInfo.CamAxis;
            if (axis != Vector2.zero)
            {
                var rotation = camFollowTargetTr.rotation;
                rotation *=
                    Quaternion.AngleAxis(axis.x * camFollowSpeed, Vector3.up);
                rotation *=
                    Quaternion.AngleAxis(-axis.y * camFollowSpeed, Vector3.right);
                
                camFollowTargetTr.rotation = rotation;

                var angles = camFollowTargetTr.localEulerAngles;
                angles.z = 0;

                var angle = angles.x;
                angles.x = ClampPitchEuler(angle, 60, 300);

                camFollowTargetTr.localEulerAngles = angles;
            }
            
            // 플레이어의 방향을 카메라 방향에 고정한다.
            transform.rotation = Quaternion.Euler(0, camFollowTargetTr.rotation.eulerAngles.y, 0);
                
            // 카메라의 로컬 y축 회전을 0으로 세팅한다. 
            camFollowTargetTr.localEulerAngles = new Vector3(camFollowTargetTr.localEulerAngles.x, 0, 0);
                    
                
            /*float yaw = Mathf.LerpAngle(transform.localRotation.eulerAngles.y, 0, Time.deltaTime * 9.0f);
            transform.localRotation = Quaternion.Euler(0, yaw, 0);*/

        }
        
        public static float ClampPitchEuler(float pitch, float min, float max)
        {
            if (pitch > 180 && pitch < max) pitch = max;
            else if(pitch < 180 && pitch > min) pitch = min;
            return pitch;
        }
    
        /*void SetPlayerRotation(Vector3 dir)
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
        }*/

        private void Cancel() {
            isMoving = false;
        }
    }
}
