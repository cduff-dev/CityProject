using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

public class PlayerController : MonoBehaviour
{

    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;
    UnityEngine.Vector2 currentMovementInput;
    UnityEngine.Vector3 currentMovement;
    UnityEngine.Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    public float movementSpeed;
    public float runMultiplier = 3.0f;
    float rotationFactorPerFrame = 15.0f;
    
    float gravity = -9.8f;
    float groundedGravity = -0.05f;


    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 2.5f;
    float maxJumpTime = 0.95f;
    bool isJumping = false;

    int isJumpingHash;
    bool isJumpAnimating = false;


    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("IsJumping");

        playerInput.CharacterControls.Movement.started += onMovementInput;
        playerInput.CharacterControls.Movement.canceled += onMovementInput;
        playerInput.CharacterControls.Movement.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setUpJumpVariables();
    }

    void setUpJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight)/ (timeToApex * timeToApex);
        initialJumpVelocity = (2 * maxJumpHeight)/ timeToApex;
    }

    void handleJump()
    {

        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity *0.5f;
            currentRunMovement.y = initialJumpVelocity *0.5f;
        }
        else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
        
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<UnityEngine.Vector2>(); 
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y !=0;

    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if(!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }


        if((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void handleRotation()
    {
        UnityEngine.Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        UnityEngine.Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
            if(positionToLookAt != UnityEngine.Vector3.zero)
            {
                UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(positionToLookAt);
                transform.rotation = UnityEngine.Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
            }
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;


        if(characterController.isGrounded)
        {
            if(isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYvelocity = currentMovement.y;
            float newYvelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYvelocity = (previousYvelocity + newYvelocity) * 0.5f;
            currentMovement.y = nextYvelocity;
            currentRunMovement.y = nextYvelocity;
        }
        else
        {
            float previousYvelocity = currentMovement.y;
            float newYvelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYvelocity = (previousYvelocity + newYvelocity) * 0.5f;
            currentMovement.y = nextYvelocity;
            currentRunMovement.y = nextYvelocity;

        }
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        

        if(isRunPressed)
        {
            characterController.Move(currentRunMovement * movementSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * movementSpeed * Time.deltaTime);
        }
        
        //Debug.Log(currentMovement);
        if(isJumpPressed)
        {
            UnityEngine.Debug.Log("jump Pressed");
        }
        handleGravity();
        handleJump();
    }


    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
