using UnityEngine;

public class MusketeerBackLeft : MusketeerBaseState
{
    public override void EnterState(MusketeerUnit unit)
    {
        unit.SetDirectionTrigger(MusketeerUnit.DirectionTriggerStates.BLeft);
    }
}