using System;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Player.Scripts.PlayerController;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera characterCamera;
    [SerializeField] private Animator playerAnimator;
    
    [Header("Base Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float drag = 0.1f;
    public float movingThreshold = 0.01f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.02f;
    
    
    [Header("Camera Settings")]
    private float lookSenseH = 0.1f;
    private float lookSenseV = 0.1f;
    public float lookLimitV = 89f;
    
    [Header("Camera Sensitivity")]
    [SerializeField] private float mouseLookSenseH = 0.1f;
    [SerializeField] private float mouseLookSenseV = 0.1f;
    [SerializeField] private float gamepadLookSenseH = 3.0f;
    [SerializeField] private float gamepadLookSenseV = 3.0f;
    
    
    [Header("Enviroment Interactions")]
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Player Components")]
    private PlayerLocomotionInput playerInput;
    private PlayerState playerState;
    
    private HealthComponent healthComponent;
    private ManaComponent manaComponent;
    
    private Vector2 cameraRotation = Vector2.zero;
    private Vector2 playerTargetRotation = Vector2.zero;
    
    private float verticalVelocity = 0;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerLocomotionInput>();
        playerState = GetComponent<PlayerState>();
    }
    
    private void Update()
    {
        UpdateLookSensitivity();
        UpdateMovementState();
        HandleVerticalMovement();
        HandleMovement();
    }
    
    private void UpdateMovementState()
    {
        bool isMoving = playerInput.MoveInput != Vector2.zero;
        bool isMovingLaterally = IsMovingLaterally();
        PlayerActionState lateralState = isMovingLaterally || isMoving ? PlayerActionState.Running : PlayerActionState.Idle;
        bool isGrounded = IsGrounded();
        playerState.SetPlayerState(lateralState);
    }

    private bool IsGrounded()
    {
        return characterController.isGrounded;
    }

    private void HandleVerticalMovement()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = 0f;
        
        verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleMovement()
    {
        // Vector3 cameraForwardXZ = new Vector3(characterCamera.transform.forward.x, 0, characterCamera.transform.forward.z).normalized;
        // Vector3 cameraRightXZ = new Vector3(characterCamera.transform.right.x, 0, characterCamera.transform.right.z).normalized;
        // Vector3 moveDirection = cameraRightXZ * playerInput.MoveInput.x + cameraForwardXZ * playerInput.MoveInput.y;
        //
        // Vector3 movementDelta = moveDirection * runAcceleration * Time.deltaTime;
        // Vector3 newVelocity = characterController.velocity + movementDelta;
        //
        // Vector3 currentDrag = newVelocity * drag * Time.deltaTime;
        // newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        // newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);
        // newVelocity.y = verticalVelocity;
        //
        // if (moveDirection.magnitude > 0.01f)
        // {
        //     characterController.Move(newVelocity * Time.deltaTime);
        // }
        
        //New Movement Logic
        Vector3 cameraForwardXZ = new Vector3(characterCamera.transform.forward.x, 0, characterCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(characterCamera.transform.right.x, 0, characterCamera.transform.right.z).normalized;
        Vector3 moveDirection = cameraRightXZ * playerInput.MoveInput.x + cameraForwardXZ * playerInput.MoveInput.y;
        moveDirection = moveDirection.normalized;
        
        Vector3 horizontalVelocity = moveDirection * runSpeed;
        
        horizontalVelocity.y = verticalVelocity;
        
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }
    
    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.y);
        return lateralVelocity.magnitude > movingThreshold;
    }
    
    private void UpdateLookSensitivity()
    {
        if (Gamepad.current != null && Gamepad.current.leftStick.IsActuated())
        {
            lookSenseH = gamepadLookSenseH;
            lookSenseV = gamepadLookSenseV;
        }
        else if (Mouse.current != null && Mouse.current.delta.IsActuated())
        {
            lookSenseH = mouseLookSenseH;
            lookSenseV = mouseLookSenseV;
        }
    }
    private void LateUpdate()
    {
        cameraRotation.x += lookSenseH * playerInput.LookInput.x ;
        cameraRotation.y = Math.Clamp(cameraRotation.y - lookSenseV * playerInput.LookInput.y, -lookLimitV, lookLimitV);
        
        playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * playerInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0, playerTargetRotation.x, 0);
        
        characterCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);
    }
}
