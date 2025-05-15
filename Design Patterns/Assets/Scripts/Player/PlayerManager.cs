using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   public float moveSpeed = 5f;
   private Animator animator;
   private CharacterController characterController;
   private Vector3 moveDirection;

   private void Awake()
   {
      animator = GetComponent<Animator>();
      characterController = GetComponent<CharacterController>();
   }
   
   private void Update()
   {
      if (moveDirection != Vector3.zero)
      {
         characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
         transform.forward = moveDirection; // Face movement direction
      }
      float speed = moveDirection.magnitude;
      animator.SetFloat("Speed", speed);
   }

   public void Move(Vector3 _moveDirection)
   {
      moveDirection = _moveDirection;
   }

   public void Stop()
   {
      moveDirection = Vector3.zero;
   }

   public void Jump()
   {
      animator.SetTrigger("isJumping");
   }
   public void ReverseMove()
   {
      animator.SetTrigger("isReversing");
   }
}
