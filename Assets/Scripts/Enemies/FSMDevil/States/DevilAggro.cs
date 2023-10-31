using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DevilAggro : DevilBaseState
{
    private Vector3 aimPosition = new Vector3(0.13f, 0.65f, -1.15f);
    private Vector3 startPos;

    private float lerpSpeed;
    private float t = 0f;

    private Collider[] colliders;
    public override void EnterState(DevilUnit unit)
    {
        Debug.Log("I am pursuing.");
        startPos = unit.SpriteTransform.localPosition;
        lerpSpeed = unit.BillboardComponent.lerpSpeed;
        unit.SpriteTransform.localPosition = aimPosition;
        //unit.BillboardComponent.boolean = true;
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
        colliders = Physics.OverlapSphere(unit.transform.position, unit.AttackRadius, unit.DetectionLayer);

        unit.Agent.SetDestination(unit.Player.position);
        ChangeDirection(unit);
        if (colliders.Length != 0)
        {
            unit.TransitionToState(unit.Attack1State);
        }

        //if (unit.SpriteTransform.localPosition != aimPosition)
        //{
        //    t += lerpSpeed * Time.deltaTime;
        //    unit.SpriteTransform.localPosition = Vector3.Lerp(startPos, aimPosition, t);
        //}
    }

    public override void LateUpdate(DevilUnit unit)
    {
        if (unit.DevilZone.colliders.Length == 0)
        {
            unit.TransitionToState(unit.ReturnState);
            unit.DevilZone.player = null;
        }
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
