using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilIdle : DevilBaseState
{
    private float timer;
    public override void EnterState(DevilUnit unit)
    {
        unit.Agent.isStopped = true;
        Debug.Log("I am idle.");
        timer = 0f;
        unit.SetAnimatorTrigger(DevilUnit.AnimatorTriggerStates.Idle);
    }

    public override void Update(DevilUnit unit)
    {
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            
            switch (Random.Range(0, 4))
            {
                case 0:
                    unit.TransitionToDirection(unit.FRightState);
                    break;
                case 1:
                    unit.TransitionToDirection(unit.FLeftState);
                    break;
                case 2:
                    unit.TransitionToDirection(unit.BRightState);
                    break;
                case 3:
                    unit.TransitionToDirection(unit.BLeftState);
                    break;
            }
            timer = 0f;
        }

        if(unit.DevilZone.player != null)
        {
            unit.Player = unit.DevilZone.player;
            unit.TransitionToState(unit.AggroState);
        }
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
}
