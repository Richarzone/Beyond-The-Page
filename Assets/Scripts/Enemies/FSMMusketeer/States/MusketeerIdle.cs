using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerIdle : MusketeerBaseState
{
    private float timer;
    public override void EnterState(MusketeerUnit unit)
    {
        timer = 0f;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(MusketeerUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= unit.waitTime)
        {
            unit.TransitionToState(unit.PatrolState);
        }
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            unit.player = collider.gameObject.transform;
            unit.TransitionToState(unit.AimState);
            unit.sphereColliderAggro.radius = 20f;
        }
    }

}
