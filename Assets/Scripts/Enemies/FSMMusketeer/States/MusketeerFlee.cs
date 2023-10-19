using UnityEngine;

public class MusketeerFlee : MusketeerBaseState
{
    public override void EnterState(MusketeerUnit unit)
    {
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Walk);
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
    }

    public override void Update(MusketeerUnit unit)
    {
    }
}