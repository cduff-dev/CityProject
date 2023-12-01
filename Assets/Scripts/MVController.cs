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


    void Awake()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("IsJumping");
    }

    public void IdleAnimationButton()
    {
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isJumpingHash, false);
    }

    public void WalkingAnimationButton()
    {
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isJumpingHash, false);
        //Debug.Log("Walking Pressed");
    }
    public void RunningAnimationButton()
    {
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, true);
        animator.SetBool(isJumpingHash, false);
    }

    public void JumpingAnimationButton()
    {
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isJumpingHash, true);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Main menu button pressed");
    }
}
