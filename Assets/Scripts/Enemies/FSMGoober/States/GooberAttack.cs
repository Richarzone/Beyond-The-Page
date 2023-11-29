using Photon.Pun;
using System.Collections;
using UnityEngine;

public class GooberAttack : GooberBaseState
{
    public override void EnterState(GooberUnit unit)
    {
        unit.Agent.speed = 3.5f;
        unit.Agent.isStopped = true;
        unit.Agent.ResetPath();
        unit.Agent.isStopped = false;
        MonoBehaviour.print(unit.Agent.pathStatus);
        unit.SetAnimatorTrigger(GooberUnit.AnimatorTriggerStates.Attack);
        
    }

    public override void LateUpdate(GooberUnit unit)
    {
    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {
    }

    public override void OnDisable(GooberUnit unit)
    {
        unit.TransitionToState("aggro");
        unit.photonView.RPC("TransitionToState", RpcTarget.All, "aggro");
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
    }

    public override void Update(GooberUnit unit)
    {
        if (unit.CanBeKnocked)
        {
            unit.TransitionToState("knocked");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "knocked");
        }
    }

    IEnumerator WaitForAnimationOfAttack(GooberUnit unit, GooberAttack state, float length)
    {
        yield return new WaitForSeconds(length);

        unit.TransitionToState("aggro");
        unit.photonView.RPC("TransitionToState", RpcTarget.All, "aggro");
    }
}