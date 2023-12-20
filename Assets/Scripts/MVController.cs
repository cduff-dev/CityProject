using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MVController : MonoBehaviour
{
    Animator animator;
     int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    //runs before start()
    void Awake()
    {
        animator = GetComponent<Animator>();
        
        //Used to reduce writing StringToHash Over and Over
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("IsJumping");
    }

    //Sets character into Idle Animation
    public void IdleAnimationButton()
    {
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isJumpingHash, false);
    }

    //Sets character into Walking Animation
    public void WalkingAnimationButton()
    {
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isJumpingHash, false);
    }

    //Sets character into Running Animation
    public void RunningAnimationButton()
    {
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, true);
        animator.SetBool(isJumpingHash, false);
    }

    //Sets character into Jumping Animation
    public void JumpingAnimationButton()
    {
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isJumpingHash, true);
    }

    //Sets Scene to Main Menu
    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
        //Debug.Log("Main menu button pressed");
    }
}
