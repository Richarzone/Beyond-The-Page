using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilBackRight : DevilBaseState
{
    public override void EnterState(DevilUnit unit)
    {
        unit.SetDirectionTrigger(DevilUnit.DirectionTriggerStates.BRight);
    }
}
