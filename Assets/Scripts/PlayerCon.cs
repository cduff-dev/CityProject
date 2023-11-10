using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Remoting.Contexts;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCon : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1;
    private UnityEngine.Vector3 moveVector;
    private float smoothTime = 0.05f;
    private float currentVelocity;
    private CharacterController characterController;
    [SerializeField]
    private Animator animator;
    private UnityEngine.Vector3 direction;
    [SerializeField] private float jumpPower;
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        ApplyGravity();
         
    }

    void Rotate()
    {
        if(moveVector.sqrMagnitude == 0) return;
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = UnityEngine.Quaternion.Euler(0.0f, angle, 0.0f);

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<UnityEngine.Vector2>();
        direction = new UnityEngine.Vector3(moveVector.x, 0, moveVector.y);
        if(direction.magnitude>0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Move()
    {
        characterController.Move(direction * moveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if(characterController.isGrounded && velocity < 0.0f)
        {
            velocity = 0.0f;
            animator.SetBool("Grounded", true);
            
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        
        direction.y = velocity;
    }

    public void OnJump()
    {
        velocity+=jumpPower;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        /*if(!context.started) return;
        if(!characterController.isGrounded) return;
        
        animator.Play("Jump");
        OnJump();
        */

        if(context.started && characterController.isGrounded)
        {
            //animator.Play("Jump");
            animator.SetBool("Grounded", false);
            OnJump();
        }
        
    }
   
}
