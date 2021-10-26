using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SylHurt : StateMachineBehaviour
{
    Sylvester _Sylvester;
    bool _IsNPC;
    bool _IsAuthorisedPlayer = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _IsNPC = animator.gameObject.layer == LayerMask.NameToLayer("NPC");
        if (!_IsNPC)
            return;
        _Sylvester = animator.GetComponent<Sylvester>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _Sylvester.HurtComplete();
    }
}
