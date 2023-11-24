using System.Collections;
using UnityEngine;

public class GubGubAttack : GubGubBaseState
{
    public override void EnterState(GubGubUnit unit)
    {
        unit.Agent.speed = 3.5f;
        unit.Agent.isStopped = true;
        unit.Agent.ResetPath();
        unit.Agent.isStopped = false;
        MonoBehaviour.print(unit.Agent.pathStatus);
        unit.SetAnimatorTrigger(GubGubUnit.AnimatorTriggerStates.Attack);
    }

    public override void LateUpdate(GubGubUnit unit)
    {
    }

    public override void OnCollisionEnter(GubGubUnit unit, Collision collider)
    {
    }

    public override void OnDisable(GubGubUnit unit)
    {
        unit.TransitionToState(unit.AggroState);
    }

    public override void OnTriggerEnter(GubGubUnit unit, Collider collider)
    {
    }

    public override void Update(GubGubUnit unit)
    {
        if (unit.CanBeKnocked)
        {
            unit.TransitionToState(unit.KnockedState);
        }
    }

    IEnumerator WaitForAnimationOfAttack(GubGubUnit unit, GubGubAttack state, float length)
    {
        yield return new WaitForSeconds(length);

        unit.TransitionToState(unit.AggroState);
    }
}