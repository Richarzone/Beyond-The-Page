using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class MusketeerFlee : MusketeerBaseState
{
    private Vector3 walkPosition = new Vector3(0f, 0.8f, -1f);
    private Vector3 startPos;

    private float lerpSpeed;
    private float t = 0.0f;

    private Vector3 fleeDirection;
    public override void EnterState(MusketeerUnit unit)
    {
        unit.agent.isStopped = false;
        unit.agent.speed = unit.fleeSpeed;
        Debug.Log("I am fleeing.");
        startPos = unit.spriteTransform.localPosition;
        unit.spriteTransform.localRotation = Quaternion.Euler(Vector3.zero);
        lerpSpeed = unit.billboardMusketeer.lerpSpeed;
        unit.billboardMusketeer.boolean = true;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(MusketeerUnit unit)
    {
        //Debug.DrawLine(unit.player.position, unit.transform.position, Color.blue, 10f);
        fleeDirection = -(unit.player.position - unit.transform.position).normalized * 10f;
        if (unit.spriteTransform.localPosition != walkPosition)
        {
            t += lerpSpeed * Time.deltaTime;
            unit.spriteTransform.localPosition = Vector3.Lerp(startPos, walkPosition, t);
        }
        else if (Vector3.Distance(unit.transform.position, unit.player.position) >= unit.attackRadius)
        {
            unit.TransitionToState(unit.AimState);
            unit.transform.LookAt(unit.player, Vector3.up);
            ChangeDirection(unit);
            unit.sphereCollider.enabled = true;
            unit.agent.isStopped = true;
        }
        else
        {
            unit.agent.SetDestination(fleeDirection);
            ChangeDirection(unit);
        }
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
    }

    private void ChangeDirection(MusketeerUnit unit)
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