using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClass : MonoBehaviour
{
    [SerializeField] protected float abilityCooldown;

    // Skill input
    protected bool skillInput;
    // Switch to detect if skill is active or not
    protected bool activeSkill;
    // Switch to control if the player can use the skill
    protected bool blockSkill;
    // ID of the button that called this ability
    protected int skillButton;

    protected CharacterClass characterClass;

    void Start()
    {
        characterClass = GetComponent<CharacterClass>();
        
        /*if (characterClass.Gl())
        {
            abilityCooldown = characterClass.AbilityCooldown();
        }*/
    }

    public virtual void UseAbility()
    {

    }

    public virtual IEnumerator TwinSpellCoroutine(CharacterClass character, TwinSpell ability)
    {
        yield return null;
    }

    public void SkillInput(bool input)
    {
        skillInput = input;
    }

    public bool GetSkillInput()
    {
        return skillInput;
    }

    public void ActiveSkill(bool active)
    {
        activeSkill = active;
    }

    public void AbilityButtonID(int id)
    {
        skillButton = id;
    }
}