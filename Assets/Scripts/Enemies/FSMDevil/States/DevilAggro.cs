using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DevilAggro : DevilBaseState
{
    public override void EnterState(DevilUnit unit)
    {
        unit.SpriteTransform.rotation = unit.transform.rotation;
        Debug.Log("I am pursuing.");
        //unit.SpriteTransform.localPosition = aimPosition;
        unit.Agent.speed = unit.WalkSpeed;
        unit.Agent.isStopped = false;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Walk);
        //if (unit.fromAttack)
        //{
        //    unit.BillboardComponent.t = 0;
        //}
    }

    public override void Update(DevilUnit unit)
    {
        unit.Agent.SetDestination(unit.Player.position);
        ChangeDirection(unit);
        //Debug.Log(unit.Colliders.Length);
    }

    public override void LateUpdate(DevilUnit unit)
    {
        if (unit.Colliders.Length != 0)
        {
            unit.Agent.isStopped = true;
            unit.TransitionToState(unit.Attack1State);
        }

        if (unit.DevilZone.Colliders.Length == 0)
        {
            unit.TransitionToState(unit.ReturnState);
            unit.DevilZone.Player = null;
        }
    }

    public override void OnTriggerEnter(DevilUnit unit, Collider collider)
    {

    }

    public override void OnCollisionEnter(DevilUnit unit, Collision collision)
    {

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
