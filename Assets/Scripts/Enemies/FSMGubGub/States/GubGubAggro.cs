using UnityEngine;

public class GubGubAggro : GubGubBaseState
{
    public override void EnterState(GubGubUnit unit)
    {
        MonoBehaviour.print("I am agro");
        unit.SetAnimatorTrigger(GubGubUnit.AnimatorTriggerStates.Walk);
    }

    public override void LateUpdate(GubGubUnit unit)
    {
    }

    public override void OnCollisionEnter(GubGubUnit unit, Collision collider)
    {

    }

    public override void OnDisable(GubGubUnit unit)
    {
    }

    public override void OnTriggerEnter(GubGubUnit unit, Collider collider)
    {
    }

    public override void Update(GubGubUnit unit)
    {
        unit.Agent.speed = unit.MoveSpeed;
        //Debug.Log(unit.agent.speed);
        unit.Agent.SetDestination(unit.Player.position);
        if (unit.Agent.velocity.x > 0)
        {
            unit.Sprite.flipX = true;
        }
        else
        {
            unit.Sprite.flipX = false;
        }

        if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= unit.AttackRadius)
        {
            //unit.agent.isStopped = true;
            unit.TransitionToState(unit.AttackState);
        }

        //MonoBehaviour.print(Vector3.Distance(unit.transform.position, unit.agent.destination));

    }
}