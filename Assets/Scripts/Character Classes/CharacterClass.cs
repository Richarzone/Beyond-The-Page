using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    [Header("Attacks")]
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float habilityCooldown;

    protected float timeBetweenAttacks;

    // Attack input
    protected bool attack;
    // Switch to control if the player can attack or not
    protected bool lockAttack;

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

    // Dodge input
    protected bool dodge;

    // Switch to control if the player can move or not
    protected bool blockMovement;
    // Switch to control if the player can rotate or not
    protected bool blockRotation;

    private void Start()
    {
        activeSkill1 = false;
    }

    virtual protected void Attack()
    {

    }

    virtual protected IEnumerator Skill1()
    {
        yield return null;
    }

    virtual protected IEnumerator Skill2()
    {
        yield return null;
    }

    virtual protected IEnumerator Skill3()
    {
        yield return null;
    }

    virtual protected IEnumerator Dodge(float dodgeDuration)
    {
        yield return null;
    }

    public void AttackInput(bool input)
    {
        attack = input;
    }

    #region Skills
    public void Skill1Active()
    {
        skillInput1 = true;
        activeSkill1 = true;
    }

    public void Skill2Active()
    {
        skillInput2 = true;
        activeSkill2 = true;
    }

    public void Skill3Active()
    {
        skillInput3 = true;
        activeSkill3 = true;
    }

    public void Skill1Deactivate()
    {
        skillInput1 = false;
    }

    public void Skill2Deactivate()
    {
        skillInput2 = false;
    }

    public void Skill3Deactivate()
    {
        skillInput3 = false;
    }
    #endregion

    public void DodgeInput(float dodgeDuration)
    {
        StartCoroutine(Dodge(dodgeDuration));
    }

    #region Properties
    public bool BlockingMovement()
    {
        return blockMovement;
    }

    public bool BlockingRotation()
    {
        return blockRotation;
    }
    #endregion
}