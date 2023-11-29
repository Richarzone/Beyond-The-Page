using Photon.Pun;
using UnityEngine;

public class MusketeerAggro : MusketeerBaseState
{
    private Vector3 walkPosition = new Vector3(0f, 0.8f, -1f);

    public override void EnterState(MusketeerUnit unit)
    {
        Debug.Log("Aggro");

        unit.Agent.speed = unit.PursueSpeed;
        unit.SpriteTransform.localRotation = Quaternion.Euler(Vector3.zero);
        unit.SpriteTransform.localPosition = walkPosition;
        unit.BillboardMusketeer.lerpInt = 0;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Walk);
        unit.Agent.isStopped = false;
    }

    public override void Update(MusketeerUnit unit)
    {
        unit.Agent.SetDestination(unit.Player.position);
        ChangeDirection(unit);
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
        if (unit.Colliders.Length != 0)
        {
            unit.SphereRadius = unit.FleeRadius;
            unit.Agent.isStopped = true;
            // unit.TransitionToState("aim");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "aim");
        }
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            // unit.TransitionToState("fleft");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "fleft");
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            // unit.TransitionToState("fright");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "fright");
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            // unit.TransitionToState("bleft");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "bleft");
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            // unit.TransitionToState("bright");
            unit.photonView.RPC("TransitionToState", RpcTarget.All, "bright");
        }
    }
}