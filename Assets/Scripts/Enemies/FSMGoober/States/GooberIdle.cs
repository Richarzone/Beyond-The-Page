using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GooberIdle : GooberBaseState
{
    private float timer;
    public override void EnterState(GooberUnit unit)
    {
        timer = 0f;
        unit.SetAnimatorTrigger(GooberUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(GooberUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= Random.Range(1f, unit.WanderTimer + 1f)) 
        {
            // unit.TransitionToState("patrol");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "patrol");
        }

        if(unit.Player != null)
        {
            // unit.TransitionToState("aggro");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "aggro");
        }
        else if (unit.StartingHealth != unit.CurrentHealth)
        {
            unit.SphereRadius = 100f;
        }
    }

    public override void LateUpdate(GooberUnit unit)
    {

    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
    }

    public override void OnDisable(GooberUnit unit)
    {
    }
}
