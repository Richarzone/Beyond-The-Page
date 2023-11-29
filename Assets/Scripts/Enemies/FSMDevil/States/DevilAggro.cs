using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DevilAggro : DevilBaseState
{
    public override void EnterState(DevilUnit unit)
    {
        unit.SpriteTransform.rotation = unit.transform.rotation;
        unit.Agent.speed = unit.WalkSpeed;
        unit.Agent.isStopped = false;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(DevilUnit unit)
    {
        unit.Agent.SetDestination(unit.Player.position);
        ChangeDirection(unit);
    }

    public override void LateUpdate(DevilUnit unit)
    {
        if (unit.Colliders.Length != 0)
        {
            unit.Agent.isStopped = true;
            // unit.TransitionToState("attak1");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "attack1");
        }

        if (unit.DevilZone.Colliders.Length == 0)
        {
            unit.TransitionToState(unit.ReturnState);
            unit.DevilZone.Player = null;
        }
    }

    public override void OnTriggerEnter(DevilUnit unit, Collider collider)
    {

    }

    public override void OnCollisionEnter(DevilUnit unit, Collision collision)
    {

    }

    public void ChangeDirection(DevilUnit unit)
    {
        if (unit.transform.eulerAngles.y > 90f && unit.transform.eulerAngles.y < 270f)
        {
            // unit.TransitionToState("fright");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "fright");

        }
        else
        {
            // unit.TransitionToState("fleft");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "fleft");
        }
    }
}
