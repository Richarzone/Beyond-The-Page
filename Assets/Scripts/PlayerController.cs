using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // References
    private Rigidbody rb;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction = new InputAction();
    private InputAction aimAction = new InputAction();
    private InputAction attackAction = new InputAction();
    private InputAction changeClassAction = new InputAction();
    private InputAction skill1Action = new InputAction();
    private InputAction skill2Action = new InputAction();
    private InputAction skill3Action = new InputAction();
    private InputAction dodgeAction = new InputAction();
    private InputAction character1Action = new InputAction();
    private InputAction character2Action = new InputAction();
    private InputAction character3Action = new InputAction();
    private InputAction character4Action = new InputAction();

    private InputAction victoryAction = new InputAction();
    private InputAction gameOverAction = new InputAction();

    private Vector2 movement;
    private Vector2 aim;
    private bool attack;
    private bool classMenuSwitch;
    private bool dodge;

    [Header("Player Settings")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] private float playerHealth;
    [SerializeField] private bool infiniteHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<GameObject> characterClasses = new List<GameObject>();
    [SerializeField] private float characterChangeCooldown;
    private CharacterClass currentCharacterClass;
    private int currentClass;

    [Header("Damage VFX")]
    [SerializeField] private Transform damageAnimPivot;
    [SerializeField] private GameObject damageNormalAnim;
    [SerializeField] private GameObject damageCritAnim;
    [SerializeField] private GameObject damageSuperAnim;
    [SerializeField] private float minimunX;
    [SerializeField] private float maximumX;

    [Header("Dodge")]
    [SerializeField] protected float dodgeSpeed;
    [SerializeField] protected float dodgeDuration;
    [SerializeField] protected float dodgeCooldown;
    private bool isDodging;

    [SerializeField] private SceneLoaderManager sceneManager;
    /*[Header("UI")]
    [SerializeField] private GameObject classMenu;*/


    private Vector3 targetPosition;

    private void Awake()
    {   
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        moveAction = playerInput.actions["Movement"];
        moveAction.performed += context => movement = context.ReadValue<Vector2>();

        aimAction = playerInput.actions["Aim"];
        aimAction.performed += context => aim = context.ReadValue<Vector2>();

        attackAction = playerInput.actions["Attack"];
        attackAction.performed += context => attack = true;
        attackAction.canceled += context => attack = false;

        changeClassAction = playerInput.actions["Change Class"];
        changeClassAction.performed += context => classMenuSwitch = true;
        changeClassAction.canceled += context => classMenuSwitch = false;

        skill1Action = playerInput.actions["Skill 1"];
        skill1Action.started += PressedSkill1;
        skill1Action.canceled += ReleasedSkill1;

        skill2Action = playerInput.actions["Skill 2"];
        skill2Action.started += PressedSkill2;
        skill2Action.canceled += ReleasedSkill2;

        skill3Action = playerInput.actions["Skill 3"];
        skill3Action.started += PressedSkill3;
        skill3Action.canceled += ReleasedSkill3;

        dodgeAction = playerInput.actions["Dodge"];
        dodgeAction.started += UseDodge;

        character1Action = playerInput.actions["Character 1"];
        character1Action.started += context => ChageClass(context, 0);

        character2Action = playerInput.actions["Character 2"];
        character2Action.started += context => ChageClass(context, 1);

        character3Action = playerInput.actions["Character 3"];
        character3Action.started += context => ChageClass(context, 2);

        character4Action = playerInput.actions["Character 4"];
        character4Action.started += context => ChageClass(context, 3);

        /*victoryAction = playerInput.actions["Victory"];
        victoryAction.started += Victory;

        gameOverAction = playerInput.actions["Game Over"];
        gameOverAction.started += GameOver;*/

        //classMenu.GetComponent<RadialMenu>().SetNumberOfSlices(characterClasses.Count);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        aimAction.Enable();
        attackAction.Enable();
        changeClassAction.Enable();
        skill1Action.Enable();
        skill2Action.Enable();
        skill3Action.Enable();
        dodgeAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        aimAction.Disable();
        attackAction.Disable();
        changeClassAction.Disable();
        skill1Action.Disable();
        skill2Action.Disable();
        skill3Action.Disable();
        dodgeAction.Disable();
    }

    private void Start()
    {
        //classMenu.SetActive(false);

        foreach (GameObject characterClass in characterClasses)
        {
            characterClass.SetActive(false);
        }

        characterClasses[currentClass].SetActive(true);
        currentCharacterClass = characterClasses[currentClass].GetComponent<CharacterClass>();
    }

    private void Update()
    {
        // If the player is dodging cancel all inputs until the dodge is over
        if (isDodging)
        {
            return;
        }

        // Rotate the player to look a the mouse position
        if (!currentCharacterClass.BlockRotation)
        {
            Rotation();
        }

        currentCharacterClass.AttackInput(attack);

        if (dodge)
        {
            StartCoroutine(Dodge());
        }
    }

    private void FixedUpdate()
    {
        // If the player is dodging cancel all inputs until the dodge is over
        if (isDodging)
        {
            return;
        }

        if (currentCharacterClass.BlockMovement)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Move the player
        Movement();
    }

    #region Movement
    private void Movement()
    {
        rb.AddForce(new Vector3(movement.x, 0f, movement.y) * movementSpeed - rb.velocity, ForceMode.VelocityChange);
    }

    private void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(aim);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
        {
            targetPosition = raycastHit.point;
        }

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        transform.LookAt(transform.position + direction, Vector3.up);
    }
    #endregion

    #region Dodge
    private void UseDodge(InputAction.CallbackContext context)
    {
        StartCoroutine(Dodge());
        currentCharacterClass.DodgeInput(dodgeDuration);
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        rb.AddForce(transform.forward.normalized * dodgeSpeed - rb.velocity, ForceMode.VelocityChange);
        dodgeAction.Disable();

        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;

        yield return new WaitForSeconds(dodgeCooldown);

        dodgeAction.Enable();
    }
    #endregion

    #region Skills
    // Pressed Skill Button
    private void PressedSkill1(InputAction.CallbackContext context)
    {
        currentCharacterClass.Skill1Active();
    }

    private void PressedSkill2(InputAction.CallbackContext context)
    {
        currentCharacterClass.Skill2Active();
    }

    private void PressedSkill3(InputAction.CallbackContext context)
    {
        currentCharacterClass.Skill3Active();
    }

    // Released Skill Button
    private void ReleasedSkill1(InputAction.CallbackContext context)
    {
        currentCharacterClass.Skill1Deactivate();
    }

    private void ReleasedSkill2(InputAction.CallbackContext context)
    {
        currentCharacterClass.Skill2Deactivate();
    }

    private void ReleasedSkill3(InputAction.CallbackContext context)
    {
        currentCharacterClass.Skill3Deactivate();
    }

    // Lock Skill Button
    public void LockSkill(int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                skill1Action.Disable();
                break;
            case 1:
                skill2Action.Disable();
                break;
            case 2:
                skill3Action.Disable();
                break;
        }
    }

    // Unlock Skill Button
    public void UnlockSkill(int buttonID)
    {
        switch (buttonID)
        {
            case 0:
                skill1Action.Enable();
                break;
            case 1:
                skill2Action.Enable();
                break;
            case 2:
                skill3Action.Enable();
                break;
        }
    }
    #endregion

    #region Change Class
    private void ChageClass(InputAction.CallbackContext context, int character)
    {
        if (!currentCharacterClass.BlockClassChange)
        {
            currentClass = character;

            currentCharacterClass.gameObject.SetActive(false);
            currentCharacterClass = characterClasses[currentClass].GetComponent<CharacterClass>();
            currentCharacterClass.gameObject.SetActive(true);

            StartCoroutine(CharacterChangeCooldown());
        }
    }

    private IEnumerator CharacterChangeCooldown()
    {
        character1Action.Disable();
        character2Action.Disable();
        character3Action.Disable();
        character4Action.Disable();

        yield return new WaitForSeconds(characterChangeCooldown);

        character1Action.Enable();
        character2Action.Enable();
        character3Action.Enable();
        character4Action.Enable();
    }
    #endregion


    #region Enemy Hit Damage
    public void DamagePlayer(float damageValue)
    {

        if (!infiniteHealth)
        {
            playerHealth -= damageValue;
        }

        // Spawn damage effect
        float randomX = Random.Range(minimunX, maximumX);

        GameObject pivotInsatnce = new GameObject("Pivot Instance");
        pivotInsatnce.transform.position = new Vector3(damageAnimPivot.position.x + randomX, damageAnimPivot.position.y, damageAnimPivot.position.z);
        pivotInsatnce.transform.SetParent(damageAnimPivot);

        GameObject damageInstance = DamageAnimationInstance(damageValue);
        damageInstance.transform.SetParent(pivotInsatnce.transform);
        damageInstance.transform.localScale = Vector3.one;
        damageInstance.GetComponent<TextMeshProUGUI>().text = damageValue.ToString();

        Destroy(pivotInsatnce, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);

        if (playerHealth <= 0)
        {
            damageAnimPivot.SetParent(null);
            Destroy(damageAnimPivot.gameObject, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
            Destroy(gameObject);
        }
    }

    private GameObject DamageAnimationInstance(float damageValue)
    {
        if (damageValue >= 3f)
        {
            return Instantiate(damageSuperAnim, transform.position, Quaternion.identity);
        }
        if (damageValue >= 2f)
        {
            return Instantiate(damageCritAnim, transform.position, Quaternion.identity);
        }
        else
        {
            return Instantiate(damageNormalAnim, transform.position, Quaternion.identity);
        }
    }

    public LayerMask GetPlayerLayer()
    {
        return playerLayer;
    }

    #endregion

    /*private void Victory(InputAction.CallbackContext context)
    {
        sceneManager.LoadVictoryScene();
    }

    private void GameOver(InputAction.CallbackContext context)
    {
        sceneManager.LoadGameOverScene();
    }*/
}