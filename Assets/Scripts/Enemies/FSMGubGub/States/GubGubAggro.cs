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
        if (collider.CompareTag("Player"))
        {
            unit.player = collider.gameObject.transform;
        }
    }

    public override void Update(GubGubUnit unit)
    {
        unit.agent.speed = unit.moveSpeed;
        //Debug.Log(unit.agent.speed);
        unit.agent.SetDestination(unit.player.position);
        if (unit.agent.velocity.x > 0)
        {
            unit.sprite.flipX = true;
        }
        else
        {
            unit.sprite.flipX = false;
        }

        if (Vector3.Distance(unit.transform.position, unit.agent.destination) <= unit.attackRadius)
        {
            //unit.agent.isStopped = true;
            unit.TransitionToState(unit.AttackState);
        }

        //MonoBehaviour.print(Vector3.Distance(unit.transform.position, unit.agent.destination));

    }
}