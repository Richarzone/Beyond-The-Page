using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilAttack1 : DevilBaseState
{
    private Vector3 aimPosition = new Vector3(0f,2.5f,-0.25f);
    private Vector3 startPos;

    private float lerpSpeed;
    private float t = 0f;

    public override void EnterState(DevilUnit unit)
    {
        unit.Agent.isStopped = true;
        unit.fromAttack = true;
        startPos = unit.SpriteTransform.localPosition;
        lerpSpeed = unit.BillboardComponent.lerpSpeed;
        unit.SpriteTransform.localPosition = aimPosition;
        unit.BillboardComponent.boolean = false;
        Debug.Log("I am attacking.");
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Attack1);
        Debug.Log(unit.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //unit.BillboardComponent.t = 0;
    }

    public override void Update(DevilUnit unit)
    {
        //if (unit.SpriteTransform.localPosition != aimPosition)
        //{
        //    t += lerpSpeed * Time.deltaTime;
        //    unit.SpriteTransform.localPosition = Vector3.Lerp(startPos, aimPosition, t);
        //}
    }

    public override void LateUpdate(DevilUnit unit)
    {

    }

    public override void OnTriggerEnter(DevilUnit unit, Collider collider)
    {

    }

    public override void OnTriggerExit(DevilUnit unit, Collider collider)
    {

    }

    public override void OnCollisionEnter(DevilUnit unit, Collision collision)
    {

    }

    public override void OnDisable(DevilUnit unit)
    {
        unit.StartCoroutine(WaitForAnimationOfAttack(unit, this, unit.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }

    IEnumerator WaitForAnimationOfAttack(DevilUnit unit, DevilAttack1 state, float length)
    {
        yield return new WaitForSeconds(length);
        unit.TransitionToState(unit.AggroState);
        //unit.BillboardComponent.t = 0;
    }
}
