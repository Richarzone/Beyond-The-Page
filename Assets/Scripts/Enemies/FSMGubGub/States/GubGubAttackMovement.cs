using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GubGubAttackMovement : StateMachineBehaviour
{
    GubGubUnit unit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (unit == null)
        {
            unit = animator.gameObject.GetComponent<GubGubUnit>();
        }
        //Vector3 direction = Vector3.MoveTowards(unit.transform.position, unit.player.position, 1f);
        Vector3 direction = unit.player.position - unit.transform.position;
        direction.y = 0f;
        //Debug.Log(direction);
        //Debug.Log(direction.normalized * unit.attackMagnitude);
        unit.agent.velocity = direction.normalized * unit.attackMagnitude;
        //Debug.Log(unit.agent.velocity);
        unit.agent.SetDestination(unit.player.position);
        //Debug.Log(unit.agent.pathStatus);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (unit.agent.velocity.x > 0)
        {
            unit.sprite.flipX = true;
        }
        else
        {
            unit.sprite.flipX = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (unit == null)
        {
            unit = animator.gameObject.GetComponent<GubGubUnit>();
        }
        Debug.Log("ONSTATEEXIT");
        unit.currentState.OnDisable(unit);
    }

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
}
