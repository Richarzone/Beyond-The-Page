using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

public class GooberUnit : MonoBehaviour
{
    [Header("Object components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rbody;
    private Transform player;
    public Transform Player
    {
        get { return player; }
        set { player = value; }
    }
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent
    {
        get { return agent; }
    }

    [Header("Sprite Components")]
    [SerializeField] private SpriteRenderer sprite;
    public SpriteRenderer Sprite
    {
        get { return sprite; }
    }

    [Header("Enemy Data")]
    [SerializeField] private float wanderTimer;
    public float WanderTimer
    {
        get { return wanderTimer; }
    }
    [SerializeField] private float moveRadius;
    public float MoveRadius
    {
        get { return moveRadius; }
    }
    [SerializeField] private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }
    [SerializeField] private float attackRadius;
    public float AttackRadius
    {
        get { return attackRadius; }
    }
    [SerializeField] private float detectionRadius;
    public float DetectionRadius
    {
        get { return detectionRadius; }
    }

    private float sphereRadius;
    public float SphereRadius
    {
        get { return sphereRadius; }
        set { sphereRadius = value; }
    }

    [Header("Attack Range")]
    [SerializeField] private float attackMagnitude;
    public float AttackMagnitude
    {
        get { return attackMagnitude; }
    }

    [SerializeField] private LayerMask playerLayer;

    private Collider[] colliders;
    public Collider[] Colliders
    {
        get { return colliders; }
    }

    private EnemyClass enemyClass;
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

    public GooberBaseState currentState;

    public readonly GooberIdle IdleState = new GooberIdle();
    public readonly GooberPatrol PatrolState = new GooberPatrol();
    public readonly GooberAggro AggroState = new GooberAggro();
    public readonly GooberAttack AttackState = new GooberAttack();

    // Start is called before the first frame update
    void Awake()
    {
        sphereRadius = detectionRadius;
        enemyClass = GetComponent<EnemyClass>();
        TransitionToState(IdleState);
    }

    private void Start()
    {
        startingHealth = enemyClass.CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = enemyClass.CurrentHealth;
        colliders = Physics.OverlapSphere(transform.position, sphereRadius, playerLayer);
        foreach (Collider collider in colliders)
        {
            player = collider.gameObject.transform;
        }
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

    internal void TransitionToState(GooberBaseState gooberState)
    {
        currentState = gooberState;
        currentState.EnterState(this);
    }

    public enum AnimatorTriggerStates { Idle = 0, Walk = 1, Attack = 2, Death = 3}
    public void SetAnimatorTrigger(AnimatorTriggerStates state)
    {
        animator.SetInteger("anim", (int)state);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
