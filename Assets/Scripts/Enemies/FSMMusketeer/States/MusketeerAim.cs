using System.Collections;
using UnityEngine;

public class MusketeerAim : MusketeerBaseState
{
    private Vector3 aimPosition = new Vector3(0f, 1.68f, 0f);
    private Vector3 startPos;

    private float lerpSpeed;
    private float t = 0.0f;

    private IEnumerator coroutine;

    public override void EnterState(MusketeerUnit unit)
    {
        Debug.Log("Aiming at player");
        startPos = unit.spriteTransform.localPosition;
        lerpSpeed = unit.billboardMusketeer.lerpSpeed;
        unit.billboardMusketeer.boolean = false;
        unit.sphereCollider.enabled = true;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Aim);
        coroutine = WaitForAimingTime(unit, this, unit.AimTime);
        unit.StartCoroutine(coroutine);

    }

    public override void Update(MusketeerUnit unit)
    {
        unit.transform.LookAt(unit.player, Vector3.up);
        ChangeDirection(unit);

        unit.agent.SetDestination(unit.player.position);
        if (unit.spriteTransform.localPosition != aimPosition)
        {
            t += lerpSpeed * Time.deltaTime;
            unit.spriteTransform.localPosition = Vector3.Lerp(startPos, aimPosition, t);
        }
        else if (Vector3.Distance(unit.transform.position, unit.agent.destination) >= unit.pursueRadius)
        {
            unit.StopCoroutine(coroutine);
            unit.TransitionToState(unit.AggroState);
            unit.sphereCollider.radius = unit.attackRadius;
        }

        //unit.spriteRotation.LookAt(unit.player, Vector3.up);
        //unit.spriteRotation.eulerAngles = unit.player.position;

    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            unit.TransitionToState(unit.FleeState);
            unit.StopCoroutine(coroutine);
            unit.player = collider.gameObject.transform;
            unit.sphereCollider.enabled = false;
        }
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        unit.spriteTransform.LookAt(unit.player, Vector3.up);
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, 90f, 0.0f);
            unit.SetAimHelper(true);
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, -90f, 0.0f);
            unit.SetAimHelper(false);
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, 90f, 0.0f);
            unit.SetAimHelper(true);
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.spriteTransform.eulerAngles += new Vector3(0.0f, -90f, 0.0f);
            unit.SetAimHelper(false);
            unit.TransitionToDirection(unit.BRightState);
        }
    }

    IEnumerator WaitForAimingTime(MusketeerUnit unit, MusketeerAim state, float aimTime)
    {
        yield return new WaitForSeconds(aimTime);
        unit.TransitionToState(unit.ShootState);
    }

}