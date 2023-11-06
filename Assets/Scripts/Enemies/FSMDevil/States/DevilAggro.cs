using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DevilAggro : DevilBaseState
{
    private Vector3 aimPosition = new Vector3(0.13f, 0.65f, -1.15f);
    private Vector3 startPos;

    private float t = 0f;
    private float lerpSpeed = 1f;
    public override void EnterState(DevilUnit unit)
    {
        Debug.Log("I am pursuing.");
        startPos = unit.SpriteTransform.localPosition;
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

        if (unit.DevilZone.colliders.Length == 0)
        {
            unit.TransitionToState(unit.ReturnState);
            unit.DevilZone.player = null;
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
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            unit.AimHelper = true;
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.AimHelper = false;
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.AimHelper = true;
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.AimHelper = false;
            unit.TransitionToDirection(unit.BRightState);
        }
    }
}
