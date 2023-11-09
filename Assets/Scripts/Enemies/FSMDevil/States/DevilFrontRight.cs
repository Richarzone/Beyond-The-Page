using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilFrontRight : DevilBaseState
{
    public override void EnterState(DevilUnit unit)
    {
        unit.SetDirectionTrigger(DevilUnit.DirectionTriggerStates.FRight);
    }
}
