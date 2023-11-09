using System.Collections;
using UnityEngine;

public class GooberAttack : GooberBaseState
{
    public override void EnterState(GooberUnit unit)
    {
        MonoBehaviour.print("I am attacking");
        //unit.agent.isStopped = false;
        unit.Agent.speed = 3.5f;
        //unit.Agent.SetDestination(unit.transform.position);
        unit.Agent.isStopped = true;
        unit.Agent.ResetPath();
        unit.Agent.isStopped = false;
        MonoBehaviour.print(unit.Agent.pathStatus);
        unit.SetAnimatorTrigger(GooberUnit.AnimatorTriggerStates.Attack);
        
    }

    public override void LateUpdate(GooberUnit unit)
    {
    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {
    }

    public override void OnDisable(GooberUnit unit)
    {
        //unit.StartCoroutine(WaitForAnimationOfAttack(unit, this, 1f));
        unit.TransitionToState(unit.AggroState);
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
    }

    public override void Update(GooberUnit unit)
    {
    }

    IEnumerator WaitForAnimationOfAttack(GooberUnit unit, GooberAttack state, float length)
    {
        //unit.agent.SetDestination(playerPosition);
        //yield return new WaitForSeconds(length/2);
        //unit.agent.velocity = playerPosition;
        yield return new WaitForSeconds(length);

        unit.TransitionToState(unit.AggroState);
    }
}