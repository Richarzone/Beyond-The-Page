using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MusketeerUnit : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rbody;
    internal Transform player;
    public NavMeshAgent agent;
    public SpriteRenderer sprite;
    public Transform spriteRotation;
    public float waitTime;
    public SphereCollider sphereColliderAggro;
    public SphereCollider sphereColliderFlee;
    public float AimTime;

    public float patrolDistance;
    internal int patrolIndex;

    public MusketeerBaseState currentState;
    public MusketeerBaseState currentDirection;

    public readonly MusketeerIdle IdleState = new MusketeerIdle();
    public readonly MusketeerPatrol PatrolState = new MusketeerPatrol();
    public readonly MusketeerAggro AggroState = new MusketeerAggro();
    public readonly MusketeerAim AimState = new MusketeerAim();
    public readonly MusketeerShoot ShootState = new MusketeerShoot();
    public readonly MusketeerStand StandState = new MusketeerStand();
    public readonly MusketeerFlee FleeState = new MusketeerFlee();
    public readonly MusketeerFrontRight FRightState = new MusketeerFrontRight();
    public readonly MusketeerBackLeft BLeftState = new MusketeerBackLeft();
    public readonly MusketeerFrontLeft FLeftState = new MusketeerFrontLeft();
    public readonly MusketeerBackRight BRightState = new MusketeerBackRight();


    // Start is called before the first frame update
    void Awake()
    {
        NavMeshPath path = new NavMeshPath();
        patrolIndex = 0;
        //agent.autoBraking = false;
        CalculatePatrolPath(path);
        TransitionToState(IdleState);
        TransitionToDirection(FRightState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this);
    }

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    void OnCollisionEnter(Collision other)
    {
        currentState.OnCollisionEnter(this, other);
    }

    internal void TransitionToState(MusketeerBaseState musketeerState)
    {
        currentState = musketeerState;
        currentState.EnterState(this);
    }

    internal void TransitionToDirection(MusketeerBaseState musketeerDirection)
    {
        currentDirection = musketeerDirection;
        currentDirection.EnterState(this);
    }

    public enum AnimatorTriggerStates { Idle = 0, Walk = 1, Aim = 2, ShootReload = 3, Stand = 4, Death = 5}
    public void SetAnimatorTrigger(AnimatorTriggerStates state)
    {
        animator.SetInteger("anim", (int)state);
    }

    public enum DirectionTriggerStates { FRight = 0, FLeft = 1, BRight = 2, BLeft = 3 }
    public void SetDirectionTrigger(DirectionTriggerStates state)
    {
        animator.SetInteger("direction", (int)state);
    }

    public Vector3[] patrolPoints;
    void CalculatePatrolPath(NavMeshPath path)
    {
        int firstDirection = Random.Range(0, 4);
        //int firstDirection = 0;

        for (int i = 0; i < 8; i += 2)
        {
            switch (firstDirection)
            {
                case 0: //North
                    agent.CalculatePath(transform.position + new Vector3(0.0f,0.0f, patrolDistance), path);
                    if(path.status == NavMeshPathStatus.PathComplete)
                    {
                        patrolPoints[i] = transform.position + new Vector3(0.0f, 0.0f, patrolDistance);
                        patrolPoints[i + 1] = transform.position;
                    }
                    
                    firstDirection++;
                    break;
                case 1: //West
                    agent.CalculatePath(transform.position + new Vector3(-patrolDistance, 0.0f, 0.0f), path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        patrolPoints[i] = transform.position + new Vector3(-patrolDistance, 0.0f, 0.0f);
                        patrolPoints[i + 1] = transform.position;
                    }
                    
                    firstDirection++;
                    break;
                case 2: //South
                    agent.CalculatePath(transform.position + new Vector3(0.0f, 0.0f, -patrolDistance), path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        patrolPoints[i] = transform.position + new Vector3(0.0f, 0.0f, -patrolDistance);
                        patrolPoints[i + 1] = transform.position;
                    }

                    firstDirection++;
                    break;
                case 3: //East
                    agent.CalculatePath(transform.position + new Vector3(patrolDistance, 0.0f, 0.0f), path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        patrolPoints[i] = transform.position + new Vector3(patrolDistance, 0.0f, 0.0f);
                        patrolPoints[i + 1] = transform.position;
                    }
                    
                    firstDirection = 0;
                    break;
            }

        }
    }
}
