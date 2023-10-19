using UnityEngine;

public class MusketeerPatrol : MusketeerBaseState
{
    public Vector3 unitDirection;
    public override void EnterState(MusketeerUnit unit)
    {
        Debug.Log("ENTERPATROL");
        if (unit.patrolIndex == 8)
        {
            unit.patrolIndex = 0;
        }
        //Debug.Log(unit.patrolPoints[unit.patrolIndex]);
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Walk);
        ChangeDirection(unit);
        unit.agent.SetDestination(unit.patrolPoints[unit.patrolIndex]);

        //unit.agent.SetDestination(new Vector3(10f, 0f, 10f));
        
    }

    public override void Update(MusketeerUnit unit)
    {
        //if (unit.agent.velocity.x < 0)
        //{
        //    unit.sprite.flipX = true;
        //}
        //else
        //{
        //    unit.sprite.flipX = false;
        //}
        //Debug.Log(unit.agent.velocity);
        if (Vector3.Distance(unit.transform.position, unit.agent.destination) <= 0.1f)
        {
            Debug.Log("REACHEDPOINT");
            unit.patrolIndex++;
            unit.TransitionToState(unit.IdleState);

        }
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            unit.agent.isStopped = true;
            unit.player = collider.gameObject.transform;
            unit.TransitionToState(unit.AimState);
            unit.sphereColliderAggro.radius = 20f;
        }
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        if (unit.patrolPoints[unit.patrolIndex].x < unit.transform.position.x)
        {
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.patrolPoints[unit.patrolIndex].x > unit.transform.position.x)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.patrolPoints[unit.patrolIndex].z > unit.transform.position.z)
        {
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.patrolPoints[unit.patrolIndex].z < unit.transform.position.z)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
    }
}