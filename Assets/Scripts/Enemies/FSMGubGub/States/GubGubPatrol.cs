using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class GubGubPatrol : GubGubBaseState
{
    private Vector3 destination;
    private IEnumerator coroutine;
    public override void EnterState(GubGubUnit unit)
    {
        destination = RandomDirection(unit, unit.MoveRadius);
        unit.Agent.SetDestination(destination);
        coroutine = NewPath(unit);
        unit.StartCoroutine(coroutine);
        unit.SetAnimatorTrigger(GubGubUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(GubGubUnit unit)
    {
        unit.AudioOnPatrol();
        if (unit.Agent.velocity.x > 0)
        {
            unit.Sprite.flipX = true;
            if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= 0.1f)
            {
                unit.StopCoroutine(coroutine);
                unit.TransitionToState(unit.IdleState);
            }
        }
        else if (unit.Agent.velocity.x < 0)
        {
            unit.Sprite.flipX = false;
            if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= 0.1f)
            {
                unit.StopCoroutine(coroutine);
                unit.TransitionToState(unit.IdleState);
            }
        }

        if(unit.Player != null)
        {
            unit.StopCoroutine(coroutine);
            unit.TransitionToState(unit.AggroState);
        }
        else if(unit.StartingHealth != unit.CurrentHealth)
        {
            unit.SphereRadius = 100f;
        }

        if (unit.CanBeKnocked)
        {
            unit.Agent.ResetPath();
            unit.StopCoroutine(coroutine);
            unit.TransitionToState(unit.KnockedState);
        }
    }

    public override void LateUpdate(GubGubUnit unit)
    {
    }

    public override void OnCollisionEnter(GubGubUnit unit, Collision collider)
    {

    }

    public override void OnTriggerEnter(GubGubUnit unit, Collider collider)
    {
    }

    public Vector3 RandomDirection(GubGubUnit unit, float moveRadius)
    {
        Vector2 randomDirection2D = Random.insideUnitCircle.normalized * moveRadius;
        Vector3 randomDirection = new Vector3(randomDirection2D.x, 0, randomDirection2D.y);
        randomDirection += unit.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, moveRadius, 1))
        {
            finalPosition = hit.position;
        }

        //MonoBehaviour.print(finalPosition);
        return finalPosition;
    }

    public override void OnDisable(GubGubUnit unit)
    {
    }

    IEnumerator NewPath(GubGubUnit unit)
    {
        yield return new WaitForSeconds(5f);
        unit.Agent.ResetPath();
        destination = RandomDirection(unit, unit.MoveRadius);
        unit.Agent.SetDestination(destination);
    }
}