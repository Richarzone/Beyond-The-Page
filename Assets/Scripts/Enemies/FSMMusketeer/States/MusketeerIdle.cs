using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerIdle : MusketeerBaseState
{
    private float timer;
    public override void EnterState(MusketeerUnit unit)
    {
        Debug.Log(unit.SphereRadius);
        timer = 0f;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(MusketeerUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= unit.WaitTime)
        {
            // unit.TransitionToState("patrol");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "patrol");
        }

        
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
        if (unit.Player != null)
        {
            unit.Agent.isStopped = true;
            unit.SphereRadius = unit.FleeRadius;
            // unit.TransitionToState("aim");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "aim");
        }
        else if (unit.StartingHealth != unit.CurrentHealth)
        {
            unit.SphereRadius = 100f;
        }
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
    }

}
