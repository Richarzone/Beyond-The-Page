using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GubGubUnit : MonoBehaviour
{
    [Header("Object components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rbody;
    public Rigidbody Rbody
    {
        get { return rbody; }
    }
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

    private bool canBeStuned;
    public bool CanBeStuned
    {
        get { return canBeStuned; }
        set { canBeStuned = value; }
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

    public GubGubBaseState currentState;

    public readonly GubGubIdle IdleState = new GubGubIdle();
    public readonly GubGubPatrol PatrolState = new GubGubPatrol();
    public readonly GubGubAggro AggroState = new GubGubAggro();
    public readonly GubGubAttack AttackState = new GubGubAttack();
    public readonly GubGubKnocked KnockedState = new GubGubKnocked();

    [Header("SFX")]
    private AudioSource audioPlayer;
    [SerializeField] private AudioClip gubgubAttack;
    [SerializeField] private AudioClip gubgubDeath;
    private float startingTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        sphereRadius = detectionRadius;
        enemyClass = GetComponent<EnemyClass>();
        TransitionToState(IdleState);
    }

    private void Start()
    {
        startingHealth = enemyClass.CurrentHealth;
        canBeKnocked = enemyClass.CanBeKnocked;
        force = enemyClass.Force;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = enemyClass.CurrentHealth;
        canBeKnocked = enemyClass.CanBeKnocked;
        canBeStuned = enemyClass.CanBeStuned;
        force = enemyClass.Force;
        colliders = Physics.OverlapSphere(transform.position, sphereRadius, playerLayer);
        foreach(Collider collider in colliders)
        {
            player = collider.gameObject.transform;
        }
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
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    public void OnDeath()
    {
        if (currentState == AggroState)
        {
            CancelInvoke("MovementAudio");
        }

        AudioOnDeath();
    }

    public void AudioOnDeath()
    {
        audioPlayer.PlayOneShot(gubgubDeath, 1);
    }
    public void AudioOnAttack()
    {
        audioPlayer.PlayOneShot(gubgubAttack, 1);
    }

    public void AudioOnAggro()
    {
        InvokeRepeating("MovementAudio", 0, 1.5f);
    }

    public void AudioOnPatrol()
    {
        MovementAudio();
    }

    private void MovementAudio()
    {
        float durationTime = 0.015f; // 15 milliseconds
        float endTime = startingTime + durationTime;

        if (endTime > audioPlayer.clip.length)
        {
            endTime = audioPlayer.clip.length;
            startingTime = 0f;
        }

        audioPlayer.time = startingTime;
        audioPlayer.SetScheduledEndTime(endTime);
        audioPlayer.PlayScheduled(AudioSettings.dspTime);
        startingTime = endTime;
    }
}
