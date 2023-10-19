public class MusketeerFrontRight : MusketeerBaseState
{
    public override void EnterState(MusketeerUnit unit)
    {
        unit.SetDirectionTrigger(MusketeerUnit.DirectionTriggerStates.FRight);
    }
}