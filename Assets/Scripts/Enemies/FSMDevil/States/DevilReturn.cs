using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilReturn : DevilBaseState
{

    public override void EnterState(DevilUnit unit)
    {
        unit.Agent.speed = unit.WalkSpeed;
        unit.Agent.isStopped = false;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(DevilUnit unit)
    {
        unit.Agent.SetDestination(unit.AggroArea.transform.position);
        ChangeDirection(unit);

        if(Vector3.Distance(unit.transform.position, unit.AggroArea.transform.position) < 0.5f)
        {
            unit.Agent.ResetPath();
            unit.transform.rotation = Quaternion.identity;
            if (unit.transform.localRotation == Quaternion.identity)
            {
                unit.TransitionToState(unit.IdleState);
            }
        }
    }

    public override void LateUpdate(DevilUnit unit)
    {
        if (unit.DevilZone.Player != null)
        {
            unit.Player = unit.DevilZone.Player;
            unit.DevilZone.transform.position = unit.Player.position;
            unit.TransitionToState(unit.AggroState);
        }
    }

    public override void OnTriggerEnter(DevilUnit unit, Collider collider)
    {

    }

    public override void OnCollisionEnter(DevilUnit unit, Collision collision)
    {
        if ((1 << collision.gameObject.layer) == unit.ProjectileLayer)
        {
            unit.DevilZone.Radius = 100f;
        }
    }

    public void ChangeDirection(DevilUnit unit)
    {
        if (unit.transform.eulerAngles.y > 90f && unit.transform.eulerAngles.y < 270f)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
        else
        {
            unit.TransitionToDirection(unit.FLeftState);
        }
    }
}
