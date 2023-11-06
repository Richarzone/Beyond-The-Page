using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MusketeerShoot : MusketeerBaseState
{
    private bool rotate;
    private Quaternion lookOnLook;
    private Quaternion lookOnLookSprite;

    private bool executeCoroutine;
    public override void EnterState(MusketeerUnit unit)
    {
        executeCoroutine = true;
        Debug.Log("I shot.");
        Debug.Log(unit.AimHelper);
        unit.SetDirection(unit.Player);
        unit.InstantiateProjectile(unit.FirePivot, unit.Player, unit.Direction);
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
            lookOnLook = Quaternion.LookRotation(unit.Player.position - unit.transform.position);
            unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, lookOnLook, Time.deltaTime);

            ReturnToAim(unit);
            unit.SpriteTransform.rotation = Quaternion.Slerp(unit.SpriteTransform.rotation, lookOnLookSprite, Time.deltaTime);
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
            unit.StartCoroutine(WaitForAim(unit, unit.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            executeCoroutine = false;
        }
        Debug.Log("DISABLED SHOOT");
    }

    public void ReturnToAim(MusketeerUnit unit)
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

    public void ReturnToAimLeft(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {

            unit.TransitionToDirection(unit.FLeftState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.TransitionToDirection(unit.BLeftState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));

        }
    }

    public void ReturnToAimRight(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.TransitionToDirection(unit.FRightState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0f, -90f, 0f));
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.TransitionToDirection(unit.BRightState);
            lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, -90f, 0.0f));

        }
    }

    IEnumerator WaitForAim(MusketeerUnit unit, float length)
    {
        yield return new WaitForSeconds(length);
        unit.TransitionToState(unit.AimState);
        unit.SphereRadius = unit.FleeRadius;
    }
}