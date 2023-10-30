using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MusketeerShoot: MusketeerBaseState
{
    private bool rotate;
    private Quaternion lookOnLook;
    private Quaternion lookOnLookSprite;

    private bool executeCoroutine;
    public override void EnterState(MusketeerUnit unit)
    {
        unit.sphereCollider.enabled = false;
        executeCoroutine = true;
        Debug.Log("I shot.");
        Debug.Log(unit.GetAimHelper());
        unit.SetDirection(unit.player);
        unit.InstantiateProjectile(unit.GetFirePivot(), unit.player, unit.GetDirection());
        rotate = false;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.ShootReload);
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
    }

    public override void Update(MusketeerUnit unit)
    {
        if (rotate)
        {
            //ChangeDirection(unit);
            lookOnLook = Quaternion.LookRotation(unit.player.position - unit.transform.position);
            unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, lookOnLook, Time.deltaTime);

            ReturnToAim(unit);
            unit.spriteTransform.rotation = Quaternion.Slerp(unit.spriteTransform.rotation, lookOnLookSprite, Time.deltaTime);
            //ReturnToAim(unit);
        }
    }

    public override void OnDisable(MusketeerUnit unit)
    {
        //unit.TransitionToState(unit.AimState);
        rotate = true;
        Debug.Log(executeCoroutine);
        if (executeCoroutine)
        {
            unit.StartCoroutine(WaitForAim(unit, unit.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            executeCoroutine = false;
        }
        Debug.Log("DISABLED SHOOT");
    }

    public void ReturnToAim(MusketeerUnit unit)
    {
        if (unit.GetAimHelper())
        {
            ReturnToAimLeft(unit);
        }
        else
        {
            ReturnToAimRight(unit);
        }
    }

    public void ReturnToAimLeft(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {

            unit.TransitionToDirection(unit.FLeftState);
            lookOnLookSprite = Quaternion.LookRotation(unit.player.position - unit.spriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.TransitionToDirection(unit.BLeftState);
            lookOnLookSprite = Quaternion.LookRotation(unit.player.position - unit.spriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));

        }
    }

    public void ReturnToAimRight(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.TransitionToDirection(unit.FRightState);
            lookOnLookSprite = Quaternion.LookRotation(unit.player.position - unit.spriteTransform.position) * Quaternion.Euler(new Vector3(0f, -90f, 0f));
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.TransitionToDirection(unit.BRightState);
            lookOnLookSprite = Quaternion.LookRotation(unit.player.position - unit.spriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, -90f, 0.0f));

        }
    }

    IEnumerator WaitForAim(MusketeerUnit unit, float length)
    {
        yield return new WaitForSeconds(length);
        unit.TransitionToState(unit.AimState);
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        unit.spriteTransform.LookAt(unit.player, Vector3.up);
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, 90f, 0.0f);
            //unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, -90f, 0.0f);
            //unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, 90f, 0.0f);
            //unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, -90f, 0.0f);
            //unit.TransitionToDirection(unit.BRightState);
        }
    }
}