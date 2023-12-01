using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

// TODO: ------ MAKE GETTERS/SETTERS FOR HEALING CATEGORY ------

public class PlayerController : MonoBehaviourPun
{
    // References
    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    private AudioSource audioSource;

    [Header("PhotonMultiplayer")]
    [SerializeField] GameObject virtualCamera;
    [SerializeField] GameObject uICanvas;
    private PhotonView view;
    

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
    private InputAction buffAction = new InputAction();
    private InputAction healAction = new InputAction();
    private InputAction pauseAction = new InputAction();

    private InputAction victoryAction = new InputAction();
    private InputAction gameOverAction = new InputAction();

    private Vector2 movement;
    private Vector2 aim;
    private bool attack;
    private bool dodge;

    [Header("Player Settings")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool infiniteHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<GameObject> characterClasses = new List<GameObject>();
    [SerializeField] private float characterChangeCooldown;
    private CharacterClass currentCharacterClass;
    private int currentClass;
    
    [Header("Health")]
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public int health;
    [SerializeField] public int extraHealth = 0;
    [SerializeField] public int usosChicharrones = 3;
    [SerializeField] private ParticleSystem healingVFX;
    [SerializeField] private ChicharronSpot chicharronSpot;

    [Header("Damage VFX")]
    [SerializeField] private Transform damageAnimPivot;
    [SerializeField] private GameObject damageNormalAnim;
    [SerializeField] private GameObject damageCritAnim;
    [SerializeField] private GameObject damageSuperAnim;
    [SerializeField] private float minimunX;
    [SerializeField] private float maximumX;

    [Header("Dodge")]
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDuration;
    [SerializeField] private float dodgeCooldown;
    private bool isDodging;
    
    [Header("Buff")]
    [SerializeField] private BuffSO currentBuff;
    [SerializeField] private Transform buffVFXpivot;

    [Header("UI Change Image Class")]
    [SerializeField] public Texture[] changeClass;
    [SerializeField] public RawImage selectedClass;

    [Header("UI Buff")]
    [SerializeField] private Image buffImage;

    [Header("UI Pause Menu")]
    [SerializeField] public GameObject GameCanvas;
    [SerializeField] public GameObject PauseCanvas;
    public bool isPaused = false;
    public int MainMenuSceneIndex;

    [SerializeField] private SceneLoaderManager sceneManager;

    private float damageMultiplier;
    private float attackSpeedMultiplier;
    private float cooldownReductionMultiplier;

    /*[Header("UI")]
    [SerializeField] private GameObject classMenu;*/

    private Vector3 targetPosition;

    [SerializeField] private GameObject arrow;

    [Header("SFX")]
    [SerializeField] private AudioClip[] dashAudios;
    [SerializeField] private AudioClip gamePauseAudio;
    [SerializeField] private AudioClip clickAudio;

    private bool isPlaying = false;



    private void Awake()
    {
        view = GetComponent<PhotonView>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;

        if (view.IsMine)
        {
            //virtualCamera.SetActive(true);
            virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Follow = transform;
            virtualCamera.GetComponent<CinemachineVirtualCamera>().m_LookAt = transform;
            uICanvas.SetActive(true);

            moveAction = playerInput.actions["Movement"];
            moveAction.performed += context => movement = context.ReadValue<Vector2>();

            aimAction = playerInput.actions["Aim"];
            aimAction.performed += context => aim = context.ReadValue<Vector2>();

            attackAction = playerInput.actions["Attack"];
            attackAction.performed += context => attack = true;
            attackAction.canceled += context => attack = false;

            skill1Action = playerInput.actions["Skill 1"];
            skill1Action.started += PressedSkill1;
            skill1Action.canceled += ReleasedSkill1;

            skill2Action = playerInput.actions["Skill 2"];
            skill2Action.started += PressedSkill2;
            skill2Action.canceled += ReleasedSkill2;

            skill3Action = playerInput.actions["Skill 3"];
            skill3Action.started += PressedSkill3;
            skill3Action.canceled += ReleasedSkill3;

            healAction = playerInput.actions["Heal"];
            healAction.started += Heal;

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

            buffAction = playerInput.actions["Buff"];
            buffAction.started += UseBuff;

            pauseAction = playerInput.actions["Pause"];
            pauseAction.started += PauseMenu;

            /*victoryAction = playerInput.actions["Victory"];
            victoryAction.started += Victory;

            gameOverAction = playerInput.actions["Game Over"];
            gameOverAction.started += GameOver;*/

            //classMenu.GetComponent<RadialMenu>().SetNumberOfSlices(characterClasses.Count);

            
        }
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
        character1Action.Enable();
        character2Action.Enable();
        character3Action.Enable();
        character4Action.Enable();

        // healingAction.Enable();
        buffAction.Enable();
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
        character1Action.Disable();
        character2Action.Disable();
        character3Action.Disable();
        character4Action.Disable();

        // healingAction.Disable();
        buffAction.Disable();
    }

    private void Start()
    {
        GameManager.Instance.Health = health;

        foreach (GameObject characterClass in characterClasses)
        {
            characterClass.SetActive(false);
        }

        characterClasses[currentClass].SetActive(true);
        currentCharacterClass = characterClasses[currentClass].GetComponent<CharacterClass>();

        damageMultiplier = 0f;
        attackSpeedMultiplier = 1f;
        cooldownReductionMultiplier = 0f;
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

        if(GameManager.Instance.EnemyAmount == 0)
        {
            arrow.SetActive(true);
            Vector3 rotation = Quaternion.LookRotation(GameManager.Instance.DoorPosition.position - arrow.transform.position, Vector3.up).eulerAngles;
            rotation.x = 0f;

            arrow.transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        // If the player is dodging cancel all inputs until the dodge is over
        if (isDodging || currentCharacterClass.IsDashing)
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
        if (!isPlaying)
        {

            if (rb.velocity != Vector3.zero)
            {
                if (!audioSource.loop)
                {
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.loop)
                {
                    audioSource.loop = false;
                    audioSource.Stop();
                }
            }
        }
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
        StartCoroutine(PlayAndWaitDash());

        StartCoroutine(Dodge());
        currentCharacterClass.DodgeInput(dodgeDuration);
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        rb.AddForce(transform.forward.normalized * dodgeSpeed - rb.velocity, ForceMode.VelocityChange);
        playerCollider.enabled = false;

        dodgeAction.Disable();

        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;
        playerCollider.enabled = true;

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

    #region Buffs
    private void UseBuff(InputAction.CallbackContext context)
    {
        if (currentBuff != null)
        {
            StartCoroutine(UseBuffCoroutine(currentBuff));
            currentBuff = null;
        }
    }

    private void ResetAllCooldowns()
    {
        currentCharacterClass.GetAbilityManager().StopAllCoroutines();

        foreach (AbilityClass ability in currentCharacterClass.GetAbilities())
        {
            ability.ResetCooldown();   
        }

        foreach (GameObject characterClass in characterClasses)
        {
            characterClass.GetComponent<CharacterClass>().ResetCharacter();
        }

        skill1Action.Enable();
        skill2Action.Enable();
        skill3Action.Enable();
    }

    private IEnumerator UseBuffCoroutine(BuffSO buff)
    {
        audioSource.PlayOneShot(buff.soundEffect);

        GameObject buffVFXInstance = Instantiate(currentBuff.buffVFX, buffVFXpivot.position, Quaternion.identity);
        buffVFXInstance.transform.parent = buffVFXpivot;
        Destroy(buffVFXInstance, 5f);

        UseBuffUI();

        switch (buff.buffType)
        {
            case BuffSO.BuffType.Damage:
                damageMultiplier += buff.effectPercentage;
                yield return new WaitForSeconds(buff.effectDuration);
                damageMultiplier -= buff.effectPercentage;
                break;
            case BuffSO.BuffType.Cooldown:
                ResetAllCooldowns();
                break;
            case BuffSO.BuffType.Speed:
                attackSpeedMultiplier += buff.effectPercentage;
                yield return new WaitForSeconds(buff.effectDuration);
                attackSpeedMultiplier -= buff.effectPercentage;
                break;
        }
    }

    private void UseBuffUI()
    {
        Color color = new Color();
        color.a = 0;
        buffImage.color = color;
        buffImage.sprite = null;
    }

    private void GrabBuffUI(Sprite buffSprite)
    {
        Color color = new Color();
        color.a = 1;
        color = Color.white;
        buffImage.color = color;
        buffImage.sprite = buffSprite;
    }
    #endregion

    #region Class Events
    // Change to RPC
    private void ChageClass(InputAction.CallbackContext context, int character)
    {
        if (!currentCharacterClass.BlockClassChange)
        {
            currentClass = character;

            currentCharacterClass.gameObject.SetActive(false);
            currentCharacterClass = characterClasses[currentClass].GetComponent<CharacterClass>();
            currentCharacterClass.gameObject.SetActive(true);
            currentCharacterClass.ChangeIcons();

            StartCoroutine(CharacterChangeCooldown());

            switch (character)
            {
                case 0:
                    selectedClass.texture = changeClass[0];
                    break;
                case 1:
                    selectedClass.texture = changeClass[1];
                    break;
                case 2:
                    selectedClass.texture = changeClass[2];
                    break;
                case 3:
                    selectedClass.texture = changeClass[3];
                    break;
            }
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

    public void StartConcotionBuff(float durationBuff, float damageBuff)
    {
        StartCoroutine(currentCharacterClass.ConcoctionBuff(durationBuff, damageBuff));
    }
    #endregion

    #region Enemy Hit Damage
    public void DamagePlayer(int damageValue)
    {
        if (!infiniteHealth)
        {
            // Verify if there is extra hearts and apply the damage
            if (extraHealth > 0)
            {
                int auxDamageValue = damageValue - extraHealth;
                extraHealth -= damageValue;
                
                if (extraHealth <= 0)
                {
                    extraHealth = 0;
                }

                if (auxDamageValue < 0)
                {
                    auxDamageValue = 0;
                }

                health -= auxDamageValue;
            }
            else
            {
                health -= damageValue;
            }

            GameManager.Instance.Health = health;
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

        if (health <= 0)
        {
            //damageAnimPivot.SetParent(null);
            //Destroy(damageAnimPivot.gameObject, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
            //Destroy(gameObject);
            health = maxHealth;
            //GameManager.Instance.Health = health;
        }
    }

    public void Heal(InputAction.CallbackContext context)
    {
        if (health != maxHealth && usosChicharrones > 0)
        {
            if (health <= 7)
            {
                health += 3;
            }
            else
            {
                health = maxHealth;
            }
            
            usosChicharrones--;

            chicharronSpot.UseChicharron();

            ParticleSystem vfxSpinInstance = Instantiate(healingVFX, currentCharacterClass.GetVFXPivot().position, healingVFX.transform.rotation);
            vfxSpinInstance.transform.parent = currentCharacterClass.GetVFXPivot();
            Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);
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

    #region Properties
    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    public CapsuleCollider GetPlayerCollider()
    {
        return playerCollider;
    }

    public float DamageMultiplier()
    {
        return damageMultiplier;
    }

    public float AttackSpeedMultiplier()
    {
        return attackSpeedMultiplier;
    }

    public float CooldownMultiplierMultiplier()
    {
        return cooldownReductionMultiplier;
    }

    public CharacterClass CurrentCharacterClass()
    {
        return currentCharacterClass;
    }

    public BuffSO GetCurrentBuff()
    {
        return currentBuff;
    }

    public void SetCurrentBuff(BuffSO value)
    {
        currentBuff = value;
        GrabBuffUI(value.buffSprite);
    }
    #endregion

    #region Pause Menu
    public void MainMenu()
    {
        audioSource.PlayOneShot(clickAudio);
        SceneManager.LoadSceneAsync(MainMenuSceneIndex);
        //Time.timeScale = 1;
        isPaused = false;

    }

    public void PauseMenu(InputAction.CallbackContext context)
    {
        audioSource.PlayOneShot(gamePauseAudio);
        PauseCanvas.SetActive(true);
        GameCanvas.SetActive(false);
        //Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        audioSource.PlayOneShot(clickAudio);
        PauseCanvas.SetActive(false);
        GameCanvas.SetActive(true);
        //Time.timeScale = 1;
        isPaused = false;
    }
    #endregion


    private IEnumerator PlayAndWaitDash()
    {
        isPlaying = true;
        audioSource.PlayOneShot(dashAudios[currentClass]);
        yield return new WaitForSeconds(dashAudios[currentClass].length);
        isPlaying = false;
    }




}