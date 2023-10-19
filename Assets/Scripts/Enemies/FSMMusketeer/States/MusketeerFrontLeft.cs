public class MusketeerFrontLeft : MusketeerBaseState
{
    public override void EnterState(MusketeerUnit unit)
    {
        unit.SetDirectionTrigger(MusketeerUnit.DirectionTriggerStates.FLeft);
    }
}