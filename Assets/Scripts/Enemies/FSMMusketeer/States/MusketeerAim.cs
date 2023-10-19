using UnityEngine;

public class MusketeerAim : MusketeerBaseState
{

    public override void EnterState(MusketeerUnit unit)
    {
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Aim);
    }

    public override void Update(MusketeerUnit unit)
    {
        Debug.Log("Aiming at player");
        //Debug.Log(unit.player.position);
        unit.transform.LookAt(unit.player, Vector3.up);
        ChangeDirection(unit);
        //unit.spriteRotation.LookAt(unit.player, Vector3.up);
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
        if(collider.gameObject.transform == unit.player)
        {
            unit.TransitionToState(unit.FleeState);
        }
        else
        {
            unit.player = collider.gameObject.transform;
        }
    }

    public void OnTriggerExit(MusketeerUnit unit, Collider collider)
    {
        if (collider.gameObject.transform == unit.player)
        {
            unit.sphereColliderAggro.radius = 10f;
            unit.TransitionToState(unit.AggroState);
        }
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.TransitionToDirection(unit.BRightState);
        }
    }
}