using UnityEngine;

public class GooberAggro : GooberBaseState
{
    public override void EnterState(GooberUnit unit)
    {
        MonoBehaviour.print("I am agro");
        unit.SetAnimatorTrigger(GooberUnit.AnimatorTriggerStates.Walk);
    }

    public override void LateUpdate(GooberUnit unit)
    {
    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {

    }

    public override void OnDisable(GooberUnit unit)
    {
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
    }

    public override void Update(GooberUnit unit)
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

        if(Vector3.Distance(unit.transform.position, unit.Agent.destination) <= unit.AttackRadius) {
            //unit.agent.isStopped = true;
            unit.TransitionToState(unit.AttackState);
        }

        //MonoBehaviour.print(Vector3.Distance(unit.transform.position, unit.agent.destination));

    }
}