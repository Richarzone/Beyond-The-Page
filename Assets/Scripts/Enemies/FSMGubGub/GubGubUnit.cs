using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GubGubUnit : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rbody;
    public CapsuleCollider capsuleCollider;
    public float wanderTimer;
    public float moveRadius;
    internal Transform player;
    public NavMeshAgent agent;
    public SpriteRenderer sprite;
    public float attackMagnitude;
    public float moveSpeed;
    public float attackRadius;

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

    internal void TransitionToState(GubGubBaseState gubGubState)
    {
        currentState = gubGubState;
        currentState.EnterState(this);
    }

    public enum AnimatorTriggerStates { Idle = 0, Walk = 1, Attack = 2, Death = 3 }
    public void SetAnimatorTrigger(AnimatorTriggerStates state)
    {
        animator.SetInteger("anim", (int)state);
    }
}
