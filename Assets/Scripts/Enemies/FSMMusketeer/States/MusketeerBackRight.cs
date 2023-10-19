public class MusketeerBackRight : MusketeerBaseState
{
    public override void EnterState(MusketeerUnit unit)
    {
        unit.SetDirectionTrigger(MusketeerUnit.DirectionTriggerStates.BRight);
    }
}