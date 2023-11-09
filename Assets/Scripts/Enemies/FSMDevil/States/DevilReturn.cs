using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilReturn : DevilBaseState
{
    private Vector3 aimPosition = new Vector3(0.13f, 0.65f, -1.15f);
    private Vector3 startPos;

    private float t = 0f;
    private float lerpSpeed = 1f;

    public override void EnterState(DevilUnit unit)
    {
        Debug.Log("I am returning.");
        startPos = unit.SpriteTransform.localPosition;
        unit.Agent.speed = unit.WalkSpeed;
        unit.Agent.isStopped = false;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(DevilUnit unit)
    {
        unit.Agent.SetDestination(unit.AggroArea.transform.position);
        ChangeDirection(unit);

        //Debug.Log(Vector3.Distance(unit.transform.position, unit.AggroArea.transform.position));
        if(Vector3.Distance(unit.transform.position, unit.AggroArea.transform.position) < 0.5f)
        {
            unit.TransitionToState(unit.IdleState);
        }


        t = lerpSpeed * Time.deltaTime;
        unit.SpriteTransform.localPosition = Vector3.Lerp(startPos, aimPosition, t);
    }

    public override void LateUpdate(DevilUnit unit)
    {
        if (unit.DevilZone.player != null)
        {
            unit.Player = unit.DevilZone.player;
            unit.DevilZone.transform.position = unit.Player.position;
            unit.TransitionToState(unit.AggroState);
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
