﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    private bool stepLeft = true;
    public void PlayPlayerStep()
    {
       
        if (FindObjectOfType<Player>().GetComponent<Controller2D>().collissions.below)
        {
            if (stepLeft)
            {
                FindObjectOfType<SoundManager>().Play("PlayerStepLeft");
            }
            else
            {
                FindObjectOfType<SoundManager>().Play("PlayerStepRight");
            }

            stepLeft = (stepLeft) ? false : true;
        }
    }

    public void PlayPlayerCrouchStep()
    {
        Debug.Log(FindObjectOfType<Player>().GetComponent<Controller2D>().collissions.below);

        if (FindObjectOfType<Player>().GetComponent<Controller2D>().collissions.below)
        {
            if (stepLeft)
            {
                FindObjectOfType<SoundManager>().Play("PlayerCrouchStepLeft");
            }
            else
            {
                FindObjectOfType<SoundManager>().Play("PlayerCrouchStepRight");
            }

            stepLeft = (stepLeft) ? false : true;
        }
    }
}
