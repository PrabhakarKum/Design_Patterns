using System;
using System.Collections;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
   [SerializeField] private float moveSpeed = 5f;
   [SerializeField] private float rotationSpeed = 3f;
   
   
   private Animator _animator;
   private CharacterController _characterController;
   private Vector3 _moveDirection;
   private Vector2 _moveInput;
   private Vector3 _targetRotationDirection;

   private void Awake()
   {
      _animator = GetComponent<Animator>();
      _characterController = GetComponent<CharacterController>();
   }
   
   private void Update()
   {
      HandleGroundedMovement();
      HandleRotation();
   }

   private void LateUpdate()
   {
      PlayerCamera.Instance.HandleAllCameraActions();
   }

   public void Move(Vector3 moveDirection, Vector2 moveInput)
   {
      _moveDirection = moveDirection;
      _moveInput = moveInput;
   }

   private void HandleGroundedMovement()
   {
      if (_moveDirection != Vector3.zero) 
         _characterController.Move(_moveDirection * (moveSpeed * Time.deltaTime));
      
      var speed = _moveDirection.magnitude;
      _animator.SetFloat("Speed", speed);
   }
   
   private void HandleRotation()
   {
      _targetRotationDirection = Vector3.zero;
      _targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * _moveInput.y;
      _targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * _moveInput.x;
      _targetRotationDirection.Normalize();
      _targetRotationDirection.y = 0;
      
      if (_targetRotationDirection == Vector3.zero)
      {
         _targetRotationDirection = transform.forward;
      }

      Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);
      Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
      transform.rotation = targetRotation;
   }

   public void Stop()
   {
      _moveDirection = Vector3.zero;
   }

   public void Jump()
   {
      _animator.SetTrigger("isJumping");
   }
   public void ReverseMove()
   {
      _animator.SetTrigger("isReversing");
   }
}
