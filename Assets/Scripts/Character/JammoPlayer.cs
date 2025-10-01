using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JammoPlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float jumpHeight = 2;

    [Header("Inputs")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
   
    private CharacterController characterController;
    private float verticalVelocity;

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
        var up = cameraTransform.up;

        forward.y = 0;
        right.y = 0;

        var moveInput = moveAction.action.ReadValue<Vector2>();

        var horizontalMovement = Vector3.zero;
        var verticalMovement = Vector3.zero;

        if (moveInput != Vector2.zero)
        {
            var moveDirection = forward * moveInput.y + right * moveInput.x;
            horizontalMovement = moveDirection * (speed * Time.deltaTime);


            var lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed);
        }

        var gravity = Physics.gravity;
        var isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            verticalVelocity = 0;

            if (jumpAction.action.triggered)
            {
                verticalVelocity = Mathf.Sqrt(2 * jumpHeight * -gravity.y);
            }
        }

        verticalVelocity += gravity.y * Time.deltaTime;

        verticalMovement = up * (verticalVelocity * Time.deltaTime);

        characterController.Move(verticalMovement + horizontalMovement);
        
        
        
    }
}