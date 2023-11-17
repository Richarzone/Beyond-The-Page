using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GubGubKnocked : GubGubBaseState
{
    private bool falling;
    public override void EnterState(GubGubUnit unit)
    {
        Debug.Log("KNOCKED");
        falling = false;
        unit.Agent.ResetPath();
        unit.Rbody.isKinematic = false;
        unit.Agent.enabled = false;
        unit.Rbody.AddForce(unit.Force, ForceMode.Force);
    }

    public override void LateUpdate(GubGubUnit unit)
    {
    }

    public override void OnCollisionEnter(GubGubUnit unit, Collision collider)
    {
    }

    public override void OnDisable(GubGubUnit unit)
    {
    }

    public override void OnTriggerEnter(GubGubUnit unit, Collider collider)
    {
    }

    public override void Update(GubGubUnit unit)
    {
        Debug.Log(unit.Rbody.velocity.y);
        if (unit.Rbody.velocity.y == 0 && falling)
        {
            unit.Rbody.isKinematic = true;
            unit.Agent.enabled = true;
            unit.EnemyClass.CanBeKnocked = false;
            unit.TransitionToState(unit.AggroState);
        }
        else if(unit.Rbody.velocity.y < 0)
        {
            falling = true;
        }

        if (unit.CurrentHealth <= 0)
        {
            unit.Rbody.isKinematic = true;
            unit.Agent.enabled = true;
            unit.Agent.isStopped = true;
        }

    }

}
