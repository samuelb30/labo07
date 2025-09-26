using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JammoPlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 0.1f;

    [Header("Inputs")]
    [SerializeField] private InputActionReference moveAction;
    private CharacterController characterController;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

    }

    private void Update()
    {
        var camera = Camera.main!;
        var cameraTransform = camera.transform;
        var forward = cameraTransform.forward;
        var right = cameraTransform.right;


        forward.y = 0;
        right.y = 0;

        var moveInput = moveAction.action.ReadValue<Vector2>();

        if (moveInput == Vector2.zero)
        {
            characterController.Move(Vector2.zero);
        }
        else
        {
            var moveDirection = forward * moveInput.y + right * moveInput.x;
            characterController.Move(moveDirection * (speed * Time.deltaTime));


            var lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed);
        }

        
        
    }
}