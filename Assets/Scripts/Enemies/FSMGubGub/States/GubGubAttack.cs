using System.Collections;
using UnityEngine;

public class GubGubAttack : GubGubBaseState
{
    public override void EnterState(GubGubUnit unit)
    {
        MonoBehaviour.print("I am attacking");
        //unit.agent.isStopped = false;
        unit.Agent.speed = 3.5f;
        //unit.Agent.SetDestination(unit.transform.position);
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
        //unit.StartCoroutine(WaitForAnimationOfAttack(unit, this, 1f));
        unit.TransitionToState(unit.AggroState);
    }

    public override void OnTriggerEnter(GubGubUnit unit, Collider collider)
    {
    }

    public override void Update(GubGubUnit unit)
    {
    }

    IEnumerator WaitForAnimationOfAttack(GubGubUnit unit, GubGubAttack state, float length)
    {
        //unit.agent.SetDestination(playerPosition);
        //yield return new WaitForSeconds(length/2);
        //unit.agent.velocity = playerPosition;
        yield return new WaitForSeconds(length);

        unit.TransitionToState(unit.AggroState);
    }
}