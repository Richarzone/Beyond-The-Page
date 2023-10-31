using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GubGubUnit : MonoBehaviour
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

    public GubGubBaseState currentState;

    public readonly GubGubIdle IdleState = new GubGubIdle();
    public readonly GubGubPatrol PatrolState = new GubGubPatrol();
    public readonly GubGubAggro AggroState = new GubGubAggro();
    public readonly GubGubAttack AttackState = new GubGubAttack();

    // Start is called before the first frame update
    void Awake()
    {
        TransitionToState(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        foreach(Collider collider in colliders)
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

    internal void TransitionToState(GubGubBaseState gooberState)
    {
        currentState = gooberState;
        currentState.EnterState(this);
    }

    public enum AnimatorTriggerStates { Idle = 0, Walk = 1, Attack = 2, Death = 3 }
    public void SetAnimatorTrigger(AnimatorTriggerStates state)
    {
        animator.SetInteger("anim", (int)state);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
