using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MusketeerUnit : MonoBehaviourPun
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
    [SerializeField] private ParticleSystem smokeVFX;
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

    private bool canBeStuned;
    public bool CanBeStuned
    {
        get { return canBeStuned; }
        set { canBeStuned = value; }
    }

    private Vector3 force;
    public Vector3 Force
    {
        get { return force; }
        set { force = value; }
    }

    public MusketeerBaseState currentState;
    public MusketeerBaseState currentDirection;
    public Vector3[] patrolPoints;

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

    [Header("SFX")]
    private AudioSource audioPlayer;
    [SerializeField] private AudioClip musketeerAttack;
    // [SerializeField] private AudioClip musketeerDeath;
    private float startingTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        sphereRadius = attackRadius;
        enemyClass = GetComponent<EnemyClass>();
        patrolIndex = 0;
        billboardMusketeer = billboard.GetComponent<BillboardMusketeer>();
        //agent.autoBraking = false;
        // TransitionToState(IdleState);
        photonView.RPC("TransitionToState", RpcTarget.All, "idle");
        // TransitionToDirection(FRightState);
        photonView.RPC("TransitionToDirection", RpcTarget.All, "fright");
    }

    private void Start()
    {
        NavMeshPath path = new NavMeshPath();
        CalculatePatrolPath(path);
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
        canBeStuned = enemyClass.CanBeStuned;
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

    [PunRPC]
    public void TransitionToState(string gooberState)
    {
        switch (gooberState)
        {
            case "idle":
                currentState = IdleState;
                currentState.EnterState(this);
                break;

            case "patrol":
                currentState = PatrolState;
                currentState.EnterState(this);
                break;

            case "aggro":
                currentState = AggroState;
                currentState.EnterState(this);
                break;

            case "aim":
                currentState = AimState;
                currentState.EnterState(this);
                break;

            case "shoot":
                currentState = ShootState;
                currentState.EnterState(this);
                break;

            case "flee":
                currentState = FleeState;
                currentState.EnterState(this);
                break;

            case "knocked":
                currentState = KnockedState;
                currentState.EnterState(this);
                break;
        }
    }

    internal void TransitionToDirection(MusketeerBaseState musketeerDirection)
    {
        currentDirection = musketeerDirection;
        currentDirection.EnterState(this);
    }

    [PunRPC]
    public void TransitionToDirection(string directionState)
    {
        switch (directionState)
        {
            case "fright":
                currentDirection = FRightState;
                currentDirection.EnterState(this);
                break;

            case "bleft":
                currentDirection = BLeftState;
                currentDirection.EnterState(this);
                break;

            case "fleft":
                currentDirection = FLeftState;
                currentDirection.EnterState(this);
                break;

            case "bright":
                currentDirection = BRightState;
                currentDirection.EnterState(this);
                break;
        }
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
        ParticleSystem VFXinstance = Instantiate(smokeVFX, pivot);
        Destroy(VFXinstance.gameObject, VFXinstance.main.duration + VFXinstance.main.startLifetime.constant);
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

    public void OnDeath()
    {
        AudioOnDeath();
    }

    public void AudioOnDeath()
    {
        // audioPlayer.PlayOneShot(musketeerDeath, 1);
    }

    public void AudioOnAttack()
    {
        audioPlayer.PlayOneShot(musketeerAttack, 1);
    }

    public void AudioOnAggro()
    {
        // InvokeRepeating("MovementAudio", 0, 1.5f);
    }

    public void AudioOnPatrol()
    {
        MovementAudio();
    }

    private void MovementAudio()
    {
        float durationTime = 0.5f; // 300 milliseconds
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

    [PunRPC]
    public void StopCorroutineNet(IEnumerator corroutine)
    {
        StopCoroutine(corroutine);
    }
}
