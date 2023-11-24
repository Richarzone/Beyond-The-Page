using UnityEngine;

public class GubGubAggro : GubGubBaseState
{
    public override void EnterState(GubGubUnit unit)
    {
        unit.SphereRadius = unit.DetectionRadius;
        unit.Agent.speed = unit.MoveSpeed;
        unit.SetAnimatorTrigger(GubGubUnit.AnimatorTriggerStates.Walk);
        unit.AudioOnAggro();
    }

    public override void LateUpdate(GubGubUnit unit)
    {
    }

    public override void OnCollisionEnter(GubGubUnit unit, Collision collider)
    {
    }

    public override void OnDisable(GubGubUnit unit)
    {
    }

    public override void OnTriggerEnter(GubGubUnit unit, Collider collider)
    {
    }

    public override void Update(GubGubUnit unit)
    {
        
        unit.Agent.SetDestination(unit.Player.position);
        if (unit.Agent.velocity.x > 0)
        {
            unit.Sprite.flipX = true;
        }
        else
        {
            unit.Sprite.flipX = false;
        }

        if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= unit.AttackRadius)
        {
            unit.CancelInvoke("MovementAudio");
            unit.TransitionToState(unit.AttackState);
        }

        if (unit.CanBeKnocked)
        {
            unit.CancelInvoke("MovementAudio");
            unit.Agent.ResetPath();
            unit.TransitionToState(unit.KnockedState);
        }

    }


    
}