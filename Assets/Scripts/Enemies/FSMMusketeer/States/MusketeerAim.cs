﻿using System.Collections;
using UnityEngine;

public class MusketeerAim : MusketeerBaseState
{
    private Vector3 aimPosition = new Vector3(0f, 1.68f, 0f);

    private IEnumerator coroutine;

    public override void EnterState(MusketeerUnit unit)
    {
        Debug.Log("Aiming at player");
        unit.BillboardMusketeer.lerpInt = 3;
        unit.SpriteTransform.localPosition = aimPosition;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Aim);
        coroutine = WaitForAimingTime(unit, unit.AimTime);
        unit.StartCoroutine(coroutine);
    }

    public override void Update(MusketeerUnit unit)
    {
        unit.transform.LookAt(unit.Player, Vector3.up);
        ChangeDirection(unit);
        unit.Agent.SetDestination(unit.Player.position);

        if (Vector3.Distance(unit.transform.position, unit.Agent.destination) >= unit.PursueRadius)
        {
            unit.StopCoroutine(coroutine);
            unit.TransitionToState(unit.AggroState);
            unit.SphereRadius = unit.AttackRadius;
        }

        //unit.spriteRotation.LookAt(unit.player, Vector3.up);
        //unit.spriteRotation.eulerAngles = unit.player.position;

    }

    public override void LateUpdate(MusketeerUnit unit)
    {
        if (unit.Colliders != null)
        {
            unit.TransitionToState(unit.FleeState);
            unit.StopCoroutine(coroutine);
        }
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        unit.SpriteTransform.LookAt(unit.Player, Vector3.up);
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            unit.SpriteTransform.eulerAngles += new Vector3(0.0f, 90f, 0.0f);
            unit.AimHelper = true;
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.SpriteTransform.eulerAngles += new Vector3(0.0f, -90f, 0.0f);
            unit.AimHelper = false;
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.SpriteTransform.eulerAngles += new Vector3(0.0f, 90f, 0.0f);
            unit.AimHelper = true;
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.SpriteTransform.eulerAngles += new Vector3(0.0f, -90f, 0.0f);
            unit.AimHelper = false;
            unit.TransitionToDirection(unit.BRightState);
        }
    }

    IEnumerator WaitForAimingTime(MusketeerUnit unit, float aimTime)
    {
        yield return new WaitForSeconds(aimTime);
        unit.TransitionToState(unit.ShootState);
    }

}