using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

public class DevilUnit : MonoBehaviour
{
    [Header("Sprite Components")]
    [SerializeField] private Animator animator;
    public Animator Animator
    {
        get { return animator; }
    }
    [SerializeField] private Transform spriteTransform;
    public Transform SpriteTransform
    {
        get { return spriteTransform; }
    }

    [Header("Object Components")]
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent
    {
        get { return agent; }
    }

    [Header("Billboard")]
    [SerializeField] private GameObject billboard;
    private BillboardDevil billboardComponent;
    public BillboardDevil BillboardComponent
    {
        get { return billboardComponent; }
    }

    [Header("Enemy Data")]
    [SerializeField] private float detectionRadius;
    public float DetectionRadius
    {
        get { return detectionRadius; }
    }
    [SerializeField] private GameObject aggroArea;
    public GameObject AggroArea
    {
        get { return aggroArea; }
    }
    private DevilZone devilZone;
    public DevilZone DevilZone
    {
        get { return devilZone; }
    }
    [SerializeField] private float walkSpeed;
    public float WalkSpeed
    {
        get { return walkSpeed; }
    }

    private Transform player;
    public Transform Player
    {
        get { return player; }
        set { player = value; }
    }
    private bool rotateHelper;
    public bool RotateHelper
    {
        get { return rotateHelper; }
        set { rotateHelper = value; }
    }

    [Header("Attack Range")]
    [SerializeField] private float attackRadius;
    public float AttackRadius
    {
        get { return attackRadius; }
    }
    [SerializeField] private LayerMask playerLayer;

    private Collider[] colliders;
    public Collider[] Colliders
    {
        get { return colliders; }
        set { colliders = value; }
    }

    private bool aimHelper;
    public bool AimHelper
    {
        get { return aimHelper; }
        set { aimHelper = value; }
    }

    public DevilBaseState currentState;
    public DevilBaseState currentDirection;

    public readonly DevilIdle IdleState = new DevilIdle();
    public readonly DevilAggro AggroState = new DevilAggro();
    public readonly DevilAttack1 Attack1State = new DevilAttack1();
    public readonly DevilAttack2 Attack2State = new DevilAttack2();
    public readonly DevilReturn ReturnState = new DevilReturn();
    public readonly DevilFrontRight FRightState = new DevilFrontRight();
    public readonly DevilBackLeft BLeftState = new DevilBackLeft();
    public readonly DevilFrontLeft FLeftState = new DevilFrontLeft();
    public readonly DevilBackRight BRightState = new DevilBackRight();

    // Start is called before the first frame update
    void Awake()
    {
        devilZone = AggroArea.GetComponent<DevilZone>();
        devilZone.radius = detectionRadius;
        billboardComponent = billboard.GetComponent<BillboardDevil>();
        TransitionToState(IdleState);
        TransitionToDirection(FLeftState);
    }

    void FixedUpdate()
    {
        colliders = Physics.OverlapSphere(transform.position, AttackRadius, playerLayer);
        foreach (Collider collider in colliders)
        {
            player = collider.gameObject.transform;
        }
        currentState.FixedUpdate(this);
    }
    // Update is called once per frame
    void Update()
    {
        currentState.Update(this);
    }

    void LateUpdate()
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

    internal void TransitionToState(DevilBaseState devilState)
    {
        currentState = devilState;
        currentState.EnterState(this);
    }

    internal void TransitionToDirection(DevilBaseState devilDirection)
    {
        currentDirection = devilDirection;
        currentDirection.EnterState(this);
    }

    public enum AnimatorTriggerStates { Idle = 0, Walk = 1, Attack1 = 2, Attack2 = 3, Death = 4 }
    public void SetAnimatorTrigger(AnimatorTriggerStates state)
    {
        animator.SetInteger("anim", (int)state);
    }

    public enum DirectionTriggerStates { FRight = 0, FLeft = 1, BRight = 2, BLeft = 3 }
    public void SetDirectionTrigger(DirectionTriggerStates state)
    {
        animator.SetInteger("direction", (int)state);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
