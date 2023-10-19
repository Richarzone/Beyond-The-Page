using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GubGubAttackEnd : StateMachineBehaviour
{
    GubGubUnit unit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("ONSTATEEXIT");
        if (unit == null)
        {
            unit = animator.gameObject.GetComponentInParent<GubGubUnit>();
        }
        unit.currentState.OnDisable(unit);
    }
}
