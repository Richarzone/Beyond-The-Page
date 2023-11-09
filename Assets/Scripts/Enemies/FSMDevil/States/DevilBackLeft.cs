using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilBackLeft : DevilBaseState
{
    public override void EnterState(DevilUnit unit)
    {
        unit.SetDirectionTrigger(DevilUnit.DirectionTriggerStates.BLeft);
    }
}
