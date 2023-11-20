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

        if (!unit.CanBeKnocked)
        {
            Vector3 direction = unit.Player.position - unit.transform.position;
            direction.y = 0f;

            //Debug.Log(direction);
            //Debug.Log(direction.normalized * unit.attackMagnitude);
            unit.Agent.velocity = direction.normalized * unit.AttackMagnitude;
            //Debug.Log(unit.agent.velocity);
            unit.Agent.SetDestination(unit.Player.position * 1.5f);
            //Debug.Log(unit.agent.pathStatus);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (unit.Agent.velocity.x > 0)
        {
            unit.Sprite.flipX = true;
        }
        else
        {
            unit.Sprite.flipX = false;
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

        if (!unit.CanBeKnocked)
        {
            unit.currentState.OnDisable(unit);
        }
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
