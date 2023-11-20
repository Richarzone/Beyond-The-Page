using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MusketeerUnit : MonoBehaviour
{
    [Header("Sprite components")] 
    [SerializeField] private Animator animator;
    public Animator Animator
    {
        get { return animator; }
    }
    [SerializeField] private Rigidbody rbody;
    public Rigidbody Rbody
    {
        get { return rbody; }
    }
    [SerializeField] private Transform spriteTransform;
    public Transform SpriteTransform
    {
        get { return spriteTransform; }
    }

    [Header("Object components")]
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent
    {
        get { return agent; }
    }
    private Transform player;
    public Transform Player
    {
        get { return player; }
        set { player = value; }
    }
    //private SpriteRenderer sprite;

    [Header("Billboard")]
    [SerializeField] private GameObject billboard;
    private BillboardMusketeer billboardMusketeer;
    public BillboardMusketeer BillboardMusketeer
    {
        get { return billboardMusketeer; }
    }

    [Header("Enemy data")]
    [SerializeField] private float waitTime;
    public float WaitTime
    {
        get { return waitTime; }
    }
    [SerializeField] private float pursueRadius;
    public float PursueRadius
    {
        get { return pursueRadius; }
    }
    [SerializeField] private float attackRadius;
    public float AttackRadius
    {
        get { return attackRadius; }
    }
    [SerializeField] private float fleeRadius;
    public float FleeRadius
    {
        get { return fleeRadius; }
    }
    [SerializeField] private float pursueSpeed;
    public float PursueSpeed
    {
        get { return pursueSpeed; }
    }
    [SerializeField] private float fleeSpeed;
    public float FleeSpeed
    {
        get { return fleeSpeed; }
    }
    [SerializeField] private float aimTime;
    public float AimTime
    {
        get { return aimTime; }
    }
    [SerializeField] private float patrolDistance;
    [SerializeField] private LayerMask playerLayer;

    [Header("Projectile")]
    [SerializeField] private Transform firePivot;
    public Transform FirePivot
    {
        get { return firePivot; }
    }
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileVelocity;
    private Vector3 targetPosition;
    private Vector3 direction;
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }
    private float targetRotation;

    private int patrolIndex;
    public int PatrolIndex
    {
        get { return patrolIndex; }
        set { patrolIndex = value; }
    }
    private bool aimHelper;
    public bool AimHelper
    {
        get { return aimHelper; }
        set { aimHelper = value; }
    }
    private float sphereRadius;
    public float SphereRadius
    {
        get { return sphereRadius; }
        set { sphereRadius = value; }
    }

    private EnemyClass enemyClass;
    public EnemyClass EnemyClass
    {
        get { return enemyClass; }
    }
    private float startingHealth;
    public float StartingHealth
    {
        get
        {
            return startingHealth;
        }
    }

    private float currentHealth;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }


    private Collider[] colliders;
    public Collider[] Colliders
    {
        get { return colliders; }
        set { colliders = value; }
    }

    private bool canBeKnocked;
    public bool CanBeKnocked
    {
        get { return canBeKnocked; }
        set { canBeKnocked = value; }
    }

    private Vector3 force;
    public Vector3 Force
    {
        get { return force; }
        set { force = value; }
    }

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
    public readonly MusketeerKnocked KnockedState = new MusketeerKnocked();


    // Start is called before the first frame update
    void Awake()
    {
        sphereRadius = attackRadius;
        enemyClass = GetComponent<EnemyClass>();
        NavMeshPath path = new NavMeshPath();
        patrolIndex = 0;
        billboardMusketeer = billboard.GetComponent<BillboardMusketeer>();
        //agent.autoBraking = false;
        CalculatePatrolPath(path);
        TransitionToState(IdleState);
        TransitionToDirection(FRightState);
    }

    private void Start()
    {
        startingHealth = enemyClass.CurrentHealth;
        canBeKnocked = enemyClass.CanBeKnocked;
        force = enemyClass.Force;
    }

    private void FixedUpdate()
    {
        colliders = Physics.OverlapSphere(transform.position, sphereRadius, playerLayer);
        foreach(Collider collider in colliders)
        {
            player = collider.gameObject.transform;
        }
        currentState.FixedUpdate(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = enemyClass.CurrentHealth;
        canBeKnocked = enemyClass.CanBeKnocked;
        force = enemyClass.Force;
        currentState.Update(this);
    }

    private void LateUpdate()
    {
        currentState.LateUpdate(this);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }

}
