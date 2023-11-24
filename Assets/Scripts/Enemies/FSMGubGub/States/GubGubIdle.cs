using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GubGubIdle : GubGubBaseState
{
    private float timer;
    public override void EnterState(GubGubUnit unit)
    {
        timer = 0f;
        unit.SetAnimatorTrigger(GubGubUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(GubGubUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= Random.Range(1f, unit.WanderTimer + 1f))
        {
            unit.TransitionToState(unit.PatrolState);
        }

        if(unit.Player != null)
        {
            unit.TransitionToState(unit.AggroState);
        }
        else if(unit.StartingHealth != unit.CurrentHealth)
        {
            unit.SphereRadius = 100f;
        }
    }

    public override void LateUpdate(GubGubUnit unit)
    {

    }

    public override void OnCollisionEnter(GubGubUnit unit, Collision collider)
    {
    }

    public override void OnTriggerEnter(GubGubUnit unit, Collider collider)
    {
    }

    public override void OnDisable(GubGubUnit unit)
    {
    }
}
