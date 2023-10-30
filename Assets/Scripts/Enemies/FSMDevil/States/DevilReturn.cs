using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilReturn : DevilBaseState
{
    public override void EnterState(DevilUnit unit)
    {
        Debug.Log("I am returning.");
        unit.Agent.speed = unit.WalkSpeed;
        unit.Agent.isStopped = false;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(DevilUnit unit)
    {
        unit.Agent.SetDestination(unit.AggroArea.transform.position);
        ChangeDirection(unit);

        if(Vector3.Distance(unit.transform.position, unit.AggroArea.transform.position) < 0.1f)
        {
            unit.TransitionToState(unit.IdleState);
        }

        if (unit.DevilZone.player != null)
        {
            unit.Player = unit.DevilZone.player;
            unit.TransitionToState(unit.AggroState);
        }
    }

    public override void LateUpdate(DevilUnit unit)
    {

    }

    public override void OnTriggerEnter(DevilUnit unit, Collider collider)
    {

    }

    public override void OnTriggerExit(DevilUnit unit, Collider collider)
    {

    }

    public override void OnCollisionEnter(DevilUnit unit, Collision collision)
    {

    }

    public void ChangeDirection(DevilUnit unit)
    {
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.TransitionToDirection(unit.BRightState);
        }
    }
}
