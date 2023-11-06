using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilAttack1 : DevilBaseState
{
    private bool inRange;
    private Vector3 aimPosition = new Vector3(0f, 2.5f, -0.25f);
    private Vector3 startPos;

    private float t = 0f;
    private float lerpSpeed = 1f;
    private bool executeAnim;

    private float timer;
    private Quaternion lookOnLook;
    private Quaternion lookOnLookSprite;

    public override void EnterState(DevilUnit unit)
    {
        //unit.fromAttack = true;
        startPos = unit.SpriteTransform.localPosition;
        executeAnim = true;
        inRange = false;
        timer = 0f;
        //unit.SpriteTransform.localPosition = aimPosition;
        Debug.Log("I am attacking.");
    }

    public override void Update(DevilUnit unit)
    {
        timer += Time.deltaTime;

        //if (unit.BillboardComponent.lerpInt == 3 && executeAnim)
        //{
        //    lookOnLook = Quaternion.LookRotation(unit.Player.position - unit.transform.position);
        //    unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, lookOnLook, Time.deltaTime);

        //    ReturnToAim(unit);
        //    unit.SpriteTransform.rotation = Quaternion.Slerp(unit.SpriteTransform.rotation, lookOnLookSprite, Time.deltaTime);
        //    if (timer >= 2f)
        //    {
        //        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Attack1);
        //        //ChangeDirection(unit);
        //        executeAnim = false;
        //    }
        //}

        t += lerpSpeed * Time.deltaTime;
        unit.SpriteTransform.localPosition = Vector3.Lerp(startPos, aimPosition, t);

        if (Vector3.Distance(unit.transform.position, unit.Player.position) >= unit.AttackRadius)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
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
        unit.StartCoroutine(WaitForAnimationOfAttack(unit, this, unit.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }

    IEnumerator WaitForAnimationOfAttack(DevilUnit unit, DevilAttack1 state, float length)
    {
        Debug.Log(inRange);
        yield return new WaitForSeconds(length);
        //Debug.Log("Attack end");
        //if (inRange)
        //{
        //    unit.TransitionToState(unit.Attack1State);
        //}
        //else
        //{
        //    unit.SpriteTransform.rotation = Quaternion.identity;
        //    unit.TransitionToState(unit.AggroState);
        //}
        //unit.BillboardComponent.t = 0;
        unit.TransitionToState(unit.AggroState);
    }

    public void ReturnToAim(DevilUnit unit)
    {
        if (unit.AimHelper)
        {
            ReturnToAimLeft(unit);
        }
        else
        {
            ReturnToAimRight(unit);
        }
    }

    public void ReturnToAimLeft(DevilUnit unit)
    {
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {

            unit.TransitionToDirection(unit.FLeftState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.TransitionToDirection(unit.BLeftState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f));

        }
    }

    public void ReturnToAimRight(DevilUnit unit)
    {
        if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.TransitionToDirection(unit.FRightState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.transform.position) * Quaternion.Euler(new Vector3(0f, -90f, 0f));
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.TransitionToDirection(unit.BRightState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.transform.position) * Quaternion.Euler(new Vector3(0f, -90f, 0f));

        }
    }
}
