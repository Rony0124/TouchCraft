using PlayerInput;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocity = 1f;
    
    private CharacterController characterController;
    private bool isMoving = false;
    
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
        transform.localRotation = Quaternion.LookRotation(direction);
        characterController.Move(transform.forward * (velocity * Time.deltaTime));
        
        isMoving = true;
    }

    private void Cancel() {
        isMoving = false;
    }
}
