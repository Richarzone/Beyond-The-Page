using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilIdle : DevilBaseState
{
    private float timer;
    public override void EnterState(DevilUnit unit)
    {
        unit.Agent.isStopped = true;
        timer = 0f;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(DevilUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    // unit.TransitionToState("fright");
                    unit.photonView.RPC("TransitionToState", RpcTarget.All, "fright");
                    break;
                case 1:
                    // unit.TransitionToState("fleft");
                    unit.photonView.RPC("TransitionToState", RpcTarget.All, "fleft");
                    break;
                case 2:
                    // unit.TransitionToState("bright");
                    unit.photonView.RPC("TransitionToState", RpcTarget.All, "bright");
                    break;
                case 3:
                    // unit.TransitionToState("bleft");
                    unit.photonView.RPC("TransitionToState", RpcTarget.All, "bleft");
                    break;
            }
            timer = 0f;
        }
    }

    public override void LateUpdate(DevilUnit unit)
    {
        if (unit.DevilZone.Player != null)
        {
            unit.DevilZone.Radius = unit.DetectionRadius;
            unit.Player = unit.DevilZone.Player;
            unit.DevilZone.transform.position = unit.Player.position;
            // unit.TransitionToState("aggro");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "aggrp");
        }
    }

    public override void OnTriggerEnter(DevilUnit unit, Collider collider)
    {
    }

    public override void OnCollisionEnter(DevilUnit unit, Collision collision)
    {
        if ((1 << collision.gameObject.layer) == unit.ProjectileLayer)
        {
            unit.DevilZone.Radius = 100f;
        }
    }
}
