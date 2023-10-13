using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GooberIdle : GooberBaseState
{
    private float timer;
    public override void EnterState(GooberUnit unit)
    {
        timer = 0f;
        MonoBehaviour.print("I am  idle");
        unit.SetAnimatorTrigger(GooberUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(GooberUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= Random.Range(1f, unit.wanderTimer)) 
        {
            unit.TransitionToState(unit.PatrolState);
        }
    }

    public override void LateUpdate(GooberUnit unit)
    {

    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            unit.player = collider.gameObject.transform;
            unit.TransitionToState(unit.AggroState);
        }
    }

    public override void OnDisable(GooberUnit unit)
    {
    }
}
