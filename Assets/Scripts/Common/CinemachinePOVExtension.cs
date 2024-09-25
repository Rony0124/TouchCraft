using Cinemachine;
using PlayerInput;
using UnityEngine;

namespace Common
{
    public class CinemachinePOVExtension : CinemachineExtension
    {
        [SerializeField] private float horizontalSpeed = 10;
        [SerializeField] private float verticalSpeed = 10;
        [SerializeField] private float clampAngle = 80;
        
        [Header("Target")]
        [SerializeField] private Transform lookTarget;
        
        private PlayerInputHandler playerInputHandler;
        private Vector3 startingRotation = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();
            playerInputHandler = PlayerInputHandler.Instance;
            
            startingRotation = transform.localRotation.eulerAngles;
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (vcam.Follow && playerInputHandler)
            {
                if (stage == CinemachineCore.Stage.Aim)
                {
                    Vector2 deltaInput = playerInputHandler.localInputInfo.CamAxis;
                    startingRotation.x += deltaInput.x * verticalSpeed;
                    startingRotation.y += deltaInput.y * horizontalSpeed;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                    
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                }
            }
        }
    }
}
