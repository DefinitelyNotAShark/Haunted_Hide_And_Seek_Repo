using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : StateMachineBehaviour
{
    private MoveGhost moveScript;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        moveScript = animator.GetComponent<MoveGhost>();//get the move script on state enter
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    { 
        moveScript.PatrolBetweenPoints();//goes between points at the index
    }
}
