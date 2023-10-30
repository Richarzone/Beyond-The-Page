using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MusketeerUnit : MonoBehaviour
{
    [Header("Sprite components")] 
    [SerializeField] public Animator animator;
    public Transform spriteTransform;

    [Header("Object components")]
    public NavMeshAgent agent;
    //private SpriteRenderer sprite;
    public SphereCollider sphereCollider;

    [Header("Billboard")]
    public GameObject billboard;
    internal BillboardMusketeer billboardMusketeer;

    [Header("Enemy data")]
    public float waitTime;
    public float pursueRadius;
    public float attackRadius;
    public float fleeRadius;
    public float pursueSpeed;
    public float fleeSpeed;
    public float AimTime;
    public float patrolDistance;

    [Header("Projectile")]
    [SerializeField] private Transform firePivot;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileVelocity;
    private Vector3 targetPosition;
    private Vector3 direction;
    private float targetRotation;

    internal Transform player;
    internal int patrolIndex;
    internal bool aimHelper;

    public MusketeerBaseState currentState;
    public MusketeerBaseState currentDirection;

    public readonly MusketeerIdle IdleState = new MusketeerIdle();
    public readonly MusketeerPatrol PatrolState = new MusketeerPatrol();
    public readonly MusketeerAggro AggroState = new MusketeerAggro();
    public readonly MusketeerAim AimState = new MusketeerAim();
    public readonly MusketeerShoot ShootState = new MusketeerShoot();
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
        billboardMusketeer = billboard.GetComponent<BillboardMusketeer>();
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

    public enum AnimatorTriggerStates { Idle = 0, Walk = 1, Aim = 2, ShootReload = 3, Death = 4}
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

    public void InstantiateProjectile(Transform pivot, Transform player, Vector3 direction)
    {
        GameObject instance = Instantiate(projectile, pivot.position, Quaternion.identity);
        instance.transform.LookAt(player, Vector3.up);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
    }

    public void SetDirection(Transform player)
    {
        // Get the direction for the projectile to fly to
        targetRotation = Mathf.Atan2(player.position.y - firePivot.position.y,
                                     player.position.x - firePivot.position.x) * Mathf.Rad2Deg;

        //Debug.Log(targetPosition);
        direction = (new Vector3(player.position.x, firePivot.position.y, player.position.z) - firePivot.position).normalized;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public Transform GetFirePivot()
    {
        return firePivot;
    }

    public bool SetAimHelper(bool _aimHelper)
    {
        return aimHelper = _aimHelper;
    }

    public bool GetAimHelper()
    {
        return aimHelper;
    }
}
