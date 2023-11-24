using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerKnocked : MusketeerBaseState
{
    private bool falling;
    public override void EnterState(MusketeerUnit unit)
    {
        falling = false;
        unit.Agent.ResetPath();
        unit.Rbody.isKinematic = false;
        unit.Agent.enabled = false;
        unit.Rbody.AddForce(unit.Force, ForceMode.Force);
    }

    public override void Update(MusketeerUnit unit)
    {
        Debug.Log(unit.Rbody.velocity.y);
        if (unit.Rbody.velocity.y == 0 && falling)
        {
            unit.Rbody.isKinematic = true;
            unit.Agent.enabled = true;
            unit.EnemyClass.CanBeKnocked = false;
            unit.TransitionToState(unit.FleeState);
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
