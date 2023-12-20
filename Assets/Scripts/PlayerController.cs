using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;
//Reference - https://www.youtube.com/watch?v=UUJMGQTT5ts&t=688s
//This class is based off the reference above with minor tweaks
public class PlayerController : MonoBehaviour
{

    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    //stores player input
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

    //Runs before start
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //Used to reduce writing StringToHash Over and Over
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("IsJumping");

        //Sets up player input callbacks. performed is added for movement as its analog and has more states than just on/off like the buttons
        playerInput.CharacterControls.Movement.started += onMovementInput;
        playerInput.CharacterControls.Movement.canceled += onMovementInput;
        playerInput.CharacterControls.Movement.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setUpJumpVariables();
    }

    //Creates variables for how long till the character reaches their max height, gravity, and for the initial velocity of the jump.
    void setUpJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight)/ (timeToApex * timeToApex);
        initialJumpVelocity = (2 * maxJumpHeight)/ timeToApex;
    }

    //Jump Functionality
    void handleJump()
    {
        //Checks if character is not already jumping, is grounded, and that the jump button has been pressed
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity *0.5f;
            currentRunMovement.y = initialJumpVelocity *0.5f;
        }
        //Checks if jump button is not pressed, character is grounded, and isJumping bool is true
        else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
        
    }

    //Input system - jump action - Reads context from game object, in this case jump button
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    //Input system - Run action - Reads context from game object, in this case Run button
    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    //Input system - Movement Input - Reads context from game object, in this case digital joystick
    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<UnityEngine.Vector2>(); 
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        //checks is there any movement present.
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y !=0;

    }


    //Character Animation System - Interacts with animation controller by using bools & Input System
    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        //Checks if movement is pressed and if character is in Walking animation
        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if(!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        //Checks if movement is pressed and if character is in Running animation
        if((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    //Handles rotation of character
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
                //Get position character needs to be rotated to.
                UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(positionToLookAt);
                //Rotates character to target rotation at set rotation rate
                transform.rotation = UnityEngine.Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
            }
        }
    }

    //Applies gravity to character
    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        //checks if character is grounded, sets jump animation to false if they are still in jump animation
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
            //calculates y velocity and applies it to character if falling
            float previousYvelocity = currentMovement.y;
            float newYvelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYvelocity = (previousYvelocity + newYvelocity) * 0.5f;
            currentMovement.y = nextYvelocity;
            currentRunMovement.y = nextYvelocity;
        }
        else
        {
            //Calculates y velocity and applies it if character is NOT falling
            float previousYvelocity = currentMovement.y;
            float newYvelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYvelocity = (previousYvelocity + newYvelocity) * 0.5f;
            currentMovement.y = nextYvelocity;
            currentRunMovement.y = nextYvelocity;

        }
    }

    //Applies movement depending on if player is running or walking
    void handleMovement()
    {
         if(isRunPressed)
        {
            characterController.Move(currentRunMovement * movementSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * movementSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame. Follows single responsibilty function
    void Update()
    {
        handleRotation();
        handleAnimation();
        handleMovement();
        handleGravity();
        handleJump();
    }


    //Allows enabling/Disabling of character input. Need to be added for input to initialise
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
