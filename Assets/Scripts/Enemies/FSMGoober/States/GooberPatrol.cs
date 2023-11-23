using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class GooberPatrol : GooberBaseState
{
    private Vector3 destination;

    private AudioSource audio;
    private float startingTime = 0f;
    public float durationTime = 0.015f; // 15 milliseconds

    public override void EnterState(GooberUnit unit)
    {
        MonoBehaviour.print("I am  patrolling");
        audio = unit.goobersSFX[0];
        destination = RandomDirection(unit, unit.MoveRadius);
        unit.SetAnimatorTrigger(GooberUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(GooberUnit unit)
    {
        unit.Agent.SetDestination(destination);
        //MonoBehaviour.print(unit.agent.velocity.magnitude);
        nextAudio();
        if (unit.Agent.velocity.x > 0)
        {
            unit.Sprite.flipX = true;
            if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= 0.1f)
            {
                unit.TransitionToState(unit.IdleState);
            }
        }
        else if (unit.Agent.velocity.x < 0)
        {
            unit.Sprite.flipX = false;
            if (Vector3.Distance(unit.transform.position, unit.Agent.destination) <= 0.1f)
            {
                unit.TransitionToState(unit.IdleState);
            }
        }

        if (unit.Player != null)
        {
            unit.TransitionToState(unit.AggroState);
        }
        else if (unit.StartingHealth != unit.CurrentHealth)
        {
            unit.SphereRadius = 100f;
        }

        //if (unit.agent.velocity.magnitude <= 0.15f)
        //{
        //    unit.TransitionToState(unit.IdleState);
        //}

        if (unit.CanBeKnocked)
        {
            unit.Agent.ResetPath();
            unit.TransitionToState(unit.KnockedState);
        }
    }

    public override void LateUpdate(GooberUnit unit)
    {
    }

    public override void OnCollisionEnter(GooberUnit unit, Collision collider)
    {
    }

    public override void OnTriggerEnter(GooberUnit unit, Collider collider)
    {
        //if (collider.CompareTag("Player"))
        //{
        //    unit.Player = collider.gameObject.transform;
        //    unit.TransitionToState(unit.AggroState);
        //}
    }

    public Vector3 RandomDirection(GooberUnit unit, float moveRadius)
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

    public override void OnDisable(GooberUnit unit)
    {
    }

    void nextAudio()
    {
        // Calculate the end time for the desired audio clip
        float endTime = startingTime + durationTime;

        // Check if the end time exceeds the audio clip length
        if (endTime > audio.clip.length)
        {
            endTime = audio.clip.length;
            startingTime = 0f;
        }

        // Set the time range for the audio clip
        audio.time = startingTime;
        audio.SetScheduledEndTime(endTime);

        // Play the audio within the specified time range
        audio.PlayScheduled(AudioSettings.dspTime);

        // Update the starting time for the next iteration
        startingTime = endTime;
    }
}