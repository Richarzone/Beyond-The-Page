using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MusketeerShoot : MusketeerBaseState
{
    private bool rotate;
    private Vector3 lookPos;
    private Vector3 lookPosSprite;
    private Quaternion lookOnLook;
    private Quaternion lookOnLookSprite;

    private bool executeCoroutine;
    public override void EnterState(MusketeerUnit unit)
    {
        executeCoroutine = true;
        unit.AudioOnAttack();
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
            lookPos = unit.Player.position - unit.transform.position;
            lookPos.y = 0;
            lookOnLook = Quaternion.LookRotation(lookPos);
            unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, lookOnLook, Time.deltaTime);
            ReturnToAim(unit);
            unit.SpriteTransform.rotation = Quaternion.Slerp(unit.SpriteTransform.rotation, lookOnLookSprite, Time.deltaTime);
            if (unit.CanBeKnocked)
            {
                unit.Agent.ResetPath();
                //unit.TransitionToState(unit.KnockedState);
                unit.photonView.RPC("TransitionToState", RpcTarget.All, "knocked");
            }
        }

    }

    public override void OnDisable(MusketeerUnit unit)
    {
        rotate = true;
        Debug.Log(executeCoroutine);
        if (executeCoroutine)
        {
            unit.StartCoroutine(WaitForAim(unit, unit.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
            executeCoroutine = false;
        }
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

            //unit.TransitionToDirection(unit.FLeftState);
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "fleft");
            lookPosSprite = unit.Player.position - unit.SpriteTransform.position;
            lookPosSprite.y = 0;
            lookOnLookSprite = Quaternion.LookRotation(lookPosSprite) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));
            //lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            //unit.TransitionToDirection(unit.BLeftState);
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "bleft");
            lookPosSprite = unit.Player.position - unit.SpriteTransform.position;
            lookPosSprite.y = 0;
            lookOnLookSprite = Quaternion.LookRotation(lookPosSprite) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));
            //lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, 90f, 0.0f));

        }
    }

    public void ReturnToAimRight(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            //unit.TransitionToDirection(unit.FRightState);
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "fright");
            lookPosSprite = unit.Player.position - unit.SpriteTransform.position;
            lookPosSprite.y = 0;
            lookOnLookSprite = Quaternion.LookRotation(lookPosSprite) * Quaternion.Euler(new Vector3(0.0f, -90f, 0.0f));
            //lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0f, -90f, 0f));
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            //unit.TransitionToDirection(unit.BRightState);
            unit.photonView.RPC("TransitionToDirection", RpcTarget.All, "bright");
            lookPosSprite = unit.Player.position - unit.SpriteTransform.position;
            lookPosSprite.y = 0;
            lookOnLookSprite = Quaternion.LookRotation(lookPosSprite) * Quaternion.Euler(new Vector3(0.0f, -90f, 0.0f));
            //lookOnLookSprite = Quaternion.LookRotation(unit.Player.position - unit.SpriteTransform.position) * Quaternion.Euler(new Vector3(0.0f, -90f, 0.0f));

        }
    }

    IEnumerator WaitForAim(MusketeerUnit unit, float length)
    {
        yield return new WaitForSeconds(length);
        //unit.TransitionToState(unit.AimState);
        unit.photonView.RPC("TransitionToState", RpcTarget.All, "aim");
    }
}