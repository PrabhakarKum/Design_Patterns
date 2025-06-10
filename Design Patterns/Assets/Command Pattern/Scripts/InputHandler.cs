using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    public PlayerLocomotionManager playerLocomotion;
    private PlayerControls _playerControls;
    private ICommand _lastCommand = null;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    
    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        
        _playerControls = new PlayerControls();
        _playerControls.Controls.Undo.performed += ctx => UndoLast();
        _playerControls.Controls.Jump.performed += ctx => HandleJump();
        _playerControls.Controls.Move.performed += ctx => HandleMove(ctx.ReadValue<Vector2>());
        _playerControls.Controls.Camera.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
        _playerControls.Controls.Move.canceled += ctx =>
        {
            
            playerLocomotion.Stop();
            _moveInput = Vector2.zero;
        };
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        HandleCameraMovementInput();
        HandleMoveInput();
    }

    private void HandleMove(Vector2 direction)
    {
        _moveInput = direction;
    }

    private void HandleMoveInput()
    {
        if (_moveInput != Vector2.zero)
        {
            _moveDirection = PlayerCamera.Instance.transform.forward * _moveInput.y ; 
            _moveDirection += PlayerCamera.Instance.transform.right * _moveInput.x; 
            _moveDirection.Normalize();
            _moveDirection.y = 0;
        
            ICommand moveCommand = new MoveCommand(playerLocomotion, _moveDirection, _moveInput);
            moveCommand.Execute();
            _lastCommand = moveCommand;
        }
    }
    
    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    private void UndoLast()
    {
        if (_lastCommand != null)
        {
            _lastCommand.Undo();
            _lastCommand = null;
        }
    }

    private void HandleJump()
    {
        ICommand jumpCommand = new JumpCommand(playerLocomotion);
        jumpCommand.Execute();
    }
}
