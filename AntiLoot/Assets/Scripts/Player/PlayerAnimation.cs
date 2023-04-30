using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private string currentState;
    private PlayerController player;

    public bool canAnimate = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        MoveAnimation();
    }

    public void SetCanAnimate(bool can)
    {
        if (can) canAnimate = true;
        else canAnimate = false;
    }

    void MoveAnimation()
    {
        /*if (Input.GetAxisRaw(PlayerInput.VERTICAL) > 0 && Input.GetAxisRaw(PlayerInput.HORIZONTAL) > 0)
            ChangeAnimationState(PlayerAnimations.NORTHEAST);
        else if (Input.GetAxisRaw(PlayerInput.VERTICAL) > 0 && Input.GetAxisRaw(PlayerInput.HORIZONTAL) < 0)
            ChangeAnimationState(PlayerAnimations.NORTHWEST);*/

        if (player.playerState == PlayerStates.Walk)
            ChangeAnimationState(PlayerAnimations.WALK);
        else if (player.playerState == PlayerStates.JumpUp)
            ChangeAnimationState(PlayerAnimations.JUMPUP);
        else if (player.playerState == PlayerStates.JumpDown)
            ChangeAnimationState(PlayerAnimations.JUMPDOWN);
        else if (player.playerState == PlayerStates.WallSlide)
            ChangeAnimationState(PlayerAnimations.WALLSLIDE);
        else
            ChangeAnimationState(PlayerAnimations.IDLE);
    }

    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
}
