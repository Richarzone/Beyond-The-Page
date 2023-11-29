using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DevilAttack1 : DevilBaseState
{
    private bool aim;
    private float timeCount;
    private Quaternion lookRotation;
    private Quaternion startPos;
    public override void EnterState(DevilUnit unit)
    {
        startPos = unit.transform.rotation;
        aim = true;
        timeCount = 0f;
        unit.Agent.isStopped = true;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Attack1);
        unit.StartCoroutine(WaitForAim(unit, unit.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }

    public override void Update(DevilUnit unit)
    {
        if (aim)
        {
            lookRotation = Quaternion.LookRotation(unit.Player.position - unit.transform.position, Vector3.up) * Quaternion.Euler(unit.OffsetRotate);
            
            timeCount += Time.deltaTime;
        }
        unit.transform.rotation = Quaternion.Slerp(startPos, lookRotation, timeCount);
    }

    public override void LateUpdate(DevilUnit unit)
    {

    }

    public override void OnTriggerEnter(DevilUnit unit, Collider collider)
    {

    }

    public override void OnCollisionEnter(DevilUnit unit, Collision collision)
    {

    }

    public override void OnDisable(DevilUnit unit)
    {
        unit.StartCoroutine(WaitForAnimationOfAttack(unit, unit.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }

    IEnumerator WaitForAim(DevilUnit unit, float length)
    {
        yield return new WaitForSeconds(length);
        aim = false;
        yield return new WaitForSeconds(0.5f);
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Attack2);
    }

    IEnumerator WaitForAnimationOfAttack(DevilUnit unit, float length)
    {
        yield return new WaitForSeconds(length);
        // unit.TransitionToState("aggro");
        unit.photonView.RPC("TransitionToState", RpcTarget.All, "aggro");
    }
}
