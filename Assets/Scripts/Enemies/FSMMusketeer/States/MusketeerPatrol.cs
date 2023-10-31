using UnityEngine;

public class MusketeerPatrol : MusketeerBaseState
{
    public Vector3 unitDirection;
    public override void EnterState(MusketeerUnit unit)
    {
        Debug.Log("I am patroling");
        if (unit.PatrolIndex == 8)
        {
            unit.PatrolIndex = 0;
        }
        //Debug.Log(unit.patrolPoints[unit.patrolIndex]);
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Walk);
        ChangeDirection(unit);
        unit.Agent.SetDestination(unit.patrolPoints[unit.PatrolIndex]);

        //unit.agent.SetDestination(new Vector3(10f, 0f, 10f));
        
    }

    public override void Update(MusketeerUnit unit)
    {
        if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= 0.1f)
        {
            Debug.Log("REACHEDPOINT");
            unit.PatrolIndex++;
            unit.TransitionToState(unit.IdleState);

        }

        if(unit.Player != null)
        {
            unit.Agent.isStopped = true;
            unit.TransitionToState(unit.AimState);
            unit.SphereRadius = unit.FleeRadius;
        }
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
        //if (collider.CompareTag("Player"))
        //{
        //    unit.agent.isStopped = true;
        //    unit.player = collider.gameObject.transform;
        //    unit.TransitionToState(unit.AimState);
        //    unit.sphereCollider.radius = unit.fleeRadius;
        //}
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        if (unit.patrolPoints[unit.PatrolIndex].x < unit.transform.position.x)
        {
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.patrolPoints[unit.PatrolIndex].x > unit.transform.position.x)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.patrolPoints[unit.PatrolIndex].z > unit.transform.position.z)
        {
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.patrolPoints[unit.PatrolIndex].z < unit.transform.position.z)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
    }
}