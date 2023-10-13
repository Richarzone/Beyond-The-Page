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
        if (collider.CompareTag("Player"))
        {
            unit.player = collider.gameObject.transform;
        }
    }

    public override void Update(GooberUnit unit)
    {
        unit.agent.SetDestination(unit.player.position);
        if (unit.agent.velocity.x > 0)
        {
            unit.sprite.flipX = true;
        }
        else
        {
            unit.sprite.flipX = false;
        }

        if(Vector3.Distance(unit.transform.position, unit.agent.destination) <= 3f) {
            //unit.agent.isStopped = true;
            unit.TransitionToState(new GooberAttack());
        }

        //MonoBehaviour.print(Vector3.Distance(unit.transform.position, unit.agent.destination));

    }
}