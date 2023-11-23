﻿using UnityEngine;

public class GooberAggro : GooberBaseState
{
    private AudioSource audio;

    public override void EnterState(GooberUnit unit)
    {
        MonoBehaviour.print("I am agro");
        unit.SphereRadius = unit.DetectionRadius;
        unit.Agent.speed = unit.MoveSpeed;
        audio = unit.goobersSFX[0];
        unit.SetAnimatorTrigger(GooberUnit.AnimatorTriggerStates.Walk);
        audio.loop = true;
        audio.Play();}

    public override void LateUpdate(GooberUnit unit)
    {
    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {

    }

    public override void OnDisable(GooberUnit unit)
    {
        GooberDeath();
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
    }

    public override void Update(GooberUnit unit)
    {
        //Debug.Log(unit.agent.speed);
        unit.Agent.SetDestination(unit.Player.position);
        if (unit.Agent.velocity.x > 0)
        {
            unit.Sprite.flipX = true;
        }
        else
        {
            unit.Sprite.flipX = false;
        }

        if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= unit.AttackRadius)
        {
            //unit.agent.isStopped = true;
            audio.loop = false;
            audio.Stop();
            unit.TransitionToState(unit.AttackState);
        }

        if (unit.CanBeKnocked)
        {
            audio.loop = false;
            audio.Stop();
            unit.Agent.ResetPath();
            unit.TransitionToState(unit.KnockedState);
        }
        //MonoBehaviour.print(Vector3.Distance(unit.transform.position, unit.agent.destination));

    }

    public void GooberDeath()
    {
        audio.loop = false;
        audio.Stop();
    }
}