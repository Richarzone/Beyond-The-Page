using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooberKnocked : GooberBaseState
{
    private bool falling;
    public override void EnterState(GooberUnit unit)
    {
        Debug.Log("KNOCKED");
        falling = false;
        unit.Agent.ResetPath();
        unit.Rbody.isKinematic = false;
        unit.Agent.enabled = false;
        unit.Rbody.AddForce(unit.Force, ForceMode.Force);
    }

    public override void LateUpdate(GooberUnit unit)
    {
    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {
    }

    public override void OnDisable(GooberUnit unit)
    {
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
    }

    public override void Update(GooberUnit unit)
    {
        Debug.Log(unit.Rbody.velocity.y);
        if (unit.Rbody.velocity.y == 0 && falling)
        {
            unit.Rbody.isKinematic = true;
            unit.Agent.enabled = true;
            unit.EnemyClass.CanBeKnocked = false;
            unit.TransitionToState(unit.AggroState);
        }
        else if (unit.Rbody.velocity.y < 0)
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
