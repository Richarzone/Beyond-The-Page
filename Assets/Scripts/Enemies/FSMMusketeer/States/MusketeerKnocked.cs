using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerKnocked : MusketeerBaseState
{
    private bool falling;
    private bool stuned;
    public override void EnterState(MusketeerUnit unit)
    {
        Debug.Log("stunned");
        falling = false;
        stuned = false;
        unit.Agent.ResetPath();
        unit.Rbody.isKinematic = false;
        unit.Agent.enabled = false;
        unit.Rbody.AddForce(unit.Force, ForceMode.Force);
    }

    public override void Update(MusketeerUnit unit)
    {
        if (unit.CanBeStuned && !stuned)
        {
            stuned = true;
            unit.StartCoroutine(Stun(unit));
        }

        if (!unit.CanBeStuned)
        {
            if (unit.Rbody.velocity.y == 0 && falling)
            {
                unit.Rbody.isKinematic = true;
                unit.Agent.enabled = true;
                unit.EnemyClass.CanBeKnocked = false;
                // unit.TransitionToState("aim");
                unit.photonView.RPC("TransitionToState", RpcTarget.All, "aim");
            }
            else if (unit.Rbody.velocity.y < 0)
            {
                falling = true;
            }
        }

        if (unit.CurrentHealth <= 0)
        {
            unit.Rbody.isKinematic = true;
            unit.Agent.enabled = true;
            unit.Agent.isStopped = true;
        }

    }

    private IEnumerator Stun(MusketeerUnit unit)
    {
        yield return new WaitForSeconds(2f);
        stuned = false;
        unit.Rbody.isKinematic = true;
        unit.Agent.enabled = true;
        unit.EnemyClass.CanBeKnocked = false;
        unit.EnemyClass.CanBeStuned = false;
        // unit.TransitionToState("aim");
        unit.photonView.RPC("TransitionToState", RpcTarget.All, "aim");
    }
}
