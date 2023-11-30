using Photon.Pun;
using UnityEngine;

public class MusketeerPatrol : MusketeerBaseState
{
    public Vector3 unitDirection;
    public override void EnterState(MusketeerUnit unit)
    {
        if (unit.PatrolIndex == 8)
        {
            unit.PatrolIndex = 0;
        }
        Debug.Log(unit.SphereRadius);
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
            unit.PatrolIndex++;
            // unit.TransitionToState("patrol");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "idle");

        }

        
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
        if (unit.Player != null)
        {
            unit.Agent.isStopped = true;
            unit.SphereRadius = unit.FleeRadius;
            // unit.TransitionToState("aim");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "aim");

        }
        else if (unit.StartingHealth != unit.CurrentHealth)
        {
            unit.SphereRadius = 100f;
        }
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        if (unit.patrolPoints[unit.PatrolIndex].x < unit.transform.position.x)
        {
            // unit.TransitionToState("fleft");
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "fleft");
        }
        else if (unit.patrolPoints[unit.PatrolIndex].x > unit.transform.position.x)
        {
            // unit.TransitionToState("fright");
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "fright");
        }
        else if (unit.patrolPoints[unit.PatrolIndex].z > unit.transform.position.z)
        {
            // unit.TransitionToState("bleft");
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "bleft");
        }
        else if (unit.patrolPoints[unit.PatrolIndex].z < unit.transform.position.z)
        {
            // unit.TransitionToState("fright");
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "fright");
        }
    }
}