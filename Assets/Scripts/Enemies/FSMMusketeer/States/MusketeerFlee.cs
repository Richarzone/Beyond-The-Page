﻿using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class MusketeerFlee : MusketeerBaseState
{
    private Vector3 walkPosition = new Vector3(0f, 0.8f, -1f);

    private Vector3 fleeDirection;
    public override void EnterState(MusketeerUnit unit)
    {
        unit.Agent.isStopped = false;
        unit.Agent.speed = unit.FleeSpeed;
        Debug.Log("I am fleeing.");
        unit.SpriteTransform.localRotation = Quaternion.Euler(Vector3.zero);
        unit.SpriteTransform.localPosition = walkPosition;
        unit.BillboardMusketeer.lerpInt = 0;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(MusketeerUnit unit)
    {
        //Debug.DrawLine(unit.player.position, unit.transform.position, Color.blue, 10f);
        fleeDirection = -(unit.Player.position - unit.transform.position).normalized * 10f;
        if (Vector3.Distance(unit.transform.position, unit.Player.position) >= unit.AttackRadius)
        {
            unit.TransitionToState(unit.AimState);
            unit.SphereRadius = unit.AttackRadius;
            unit.Agent.isStopped = true;
        }
        else
        {
            unit.Agent.SetDestination(fleeDirection);
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