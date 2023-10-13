using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerIdle : MusketeerBaseState
{
    public override void EnterState(MusketeerUnit unit)
    {
        //unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Idle)
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
    }

    public override void OnCollisionEnter(MusketeerUnit unit)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit)
    {
    }

    public override void Update(MusketeerUnit unit)
    {
    }

}
