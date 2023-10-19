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
        MonoBehaviour.print("I am  idle");
        unit.SetAnimatorTrigger(GubGubUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(GubGubUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= Random.Range(1f, unit.wanderTimer + 1f))
        {
            unit.TransitionToState(unit.PatrolState);
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
        if (collider.CompareTag("Player"))
        {
            unit.player = collider.gameObject.transform;
            unit.TransitionToState(unit.AggroState);
        }
    }

    public override void OnDisable(GubGubUnit unit)
    {
    }
}
