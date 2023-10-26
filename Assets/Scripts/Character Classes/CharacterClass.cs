using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform vfxPivot;

    [Header("Basic Attack")]
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;

    [Header("Abilities")]
    [SerializeField] protected bool globalCooldown;
    [SerializeField] protected float abilityCooldown;
    [SerializeField] protected List<AbilityClass> abilities = new List<AbilityClass>();

    protected float timeBetweenAttacks;

    // Attack input
    protected bool attack;
    // Switch to control if the player can attack or not
    protected bool lockAttack;

    // ---- CAN REMOVE ----
    // Skill 1 input
    protected bool skillInput1;
    // Switch to detect if skill 1 is active or not
    protected bool activeSkill1;
    // Switch to control if the player can use the skill 1
    protected bool lockSkill1;

    // Skill 2 input
    protected bool skillInput2;
    // Switch to detect if skill 2 is active or not
    protected bool activeSkill2;
    // Switch to control if the player can use the skill 2
    protected bool lockSkill2;

    // Skill 3 input
    protected bool skillInput3;
    // Switch to detect if skill 3 is active or not
    protected bool activeSkill3;
    // Switch to control if the player can use the skill 3
    protected bool lockSkill3;
    // ---- CAN REMOVE ----

    // Dodge input
    protected bool dodge;

    // Switch to control if the player can move or not
    protected bool blockMovement;
    // Switch to control if the player can rotate or not
    protected bool blockRotation;
    // Switch to control if the player can use skills or not
    protected bool blockAbilities;
    // Switch to control if the player can switch classes or not
    protected bool blockClassChange;
    // Switch to control if the player can switch classes or not
    protected bool blockDodge;

    private AbilityManager abilityManager;

    private void Start()
    {
        abilityManager = transform.parent.GetComponent<AbilityManager>();
        animator = GetComponent<Animator>();
    }

    virtual protected void Attack()
    {

    }

    virtual protected IEnumerator Dodge(float dodgeDuration)
    {
        yield return null;
    }

    #region Inputs
    public void AttackInput(bool input)
    {
        attack = input;
    }

    public void Skill1Active()
    {
        abilities[0].SkillInput(true);
        abilities[0].ActiveSkill(true);
        abilities[0].UseAbility();
        abilityManager.LastUsedSkill = abilities[0];
    }

    public void Skill2Active()
    {
        abilities[1].SkillInput(true);
        abilities[1].ActiveSkill(true);
        abilities[1].UseAbility();
        abilityManager.LastUsedSkill = abilities[1];
    }

    public void Skill3Active()
    {
        abilities[2].SkillInput(true);
        abilities[2].ActiveSkill(true);
        abilities[2].UseAbility();
        abilityManager.LastUsedSkill = abilities[2];
    }

    public void Skill1Deactivate()
    {
        abilities[0].SkillInput(false);
    }

    public void Skill2Deactivate()
    {
        abilities[1].SkillInput(false);
    }

    public void Skill3Deactivate()
    {
        abilities[2].SkillInput(false);
    }

    public void DodgeInput(float dodgeDuration)
    {
        StartCoroutine(Dodge(dodgeDuration));
    }
    #endregion

    #region Properties
    public Animator GetAnimator()
    {
        return animator;
    }

    public Transform GetVFXPivot()
    {
        return vfxPivot;
    }

    public LayerMask GetEnemyLayer()
    {
        return enemyLayer;
    }

    public LayerMask GetGroundLayer()
    {
        return groundLayer;
    }

    public AbilityManager AbilityManager()
    {
        return abilityManager;
    }

    public float AttackDamage()
    {
        return attackDamage;
    }

    public float AbilityCooldown()
    {
        return abilityCooldown;
    }

    public bool GlobalCooldown()
    {
        return globalCooldown;
    }

    public bool BlockMovement
    {
        get { return blockMovement; }
        set { blockMovement = value; }
    }

    public bool BlockRotation
    {
        get { return blockRotation; }
        set { blockRotation = value; }
    }

    public bool BlockClassChange
    {
        get { return blockClassChange; }
        set { blockClassChange = value; }
    }

    public bool BlockAbilities
    {
        get { return blockAbilities; }
        set { blockAbilities = value; }
    }

    public bool BlockDodge
    {
        get { return blockDodge; }
        set { blockDodge = value; }
    }
    #endregion

    #region Events
    // Events for movement
    private void EnableMovement()
    {
        blockMovement = false;
    }

    private void DisableMovement()
    {
        blockMovement = true;
    }

    // Events for rotation
    private void EnableRotation()
    {
        blockRotation = false;
    }

    private void DisableRotation()
    {
        blockRotation = true;
    }

    // Events for abilities
    private void EnableAbilities()
    {
        blockAbilities = false;
    }

    private void DisableAbilities()
    {
        blockAbilities = true;
    }

    // Events for class change
    private void EnableClassChange()
    {
        blockClassChange = false;
    }

    private void DisableClassChange()
    {
        blockClassChange = true;
    }
    #endregion
}
