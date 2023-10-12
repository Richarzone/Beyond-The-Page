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
    protected bool lockSkill;

    protected CharacterClass characterClass;

    void Start()
    {
        characterClass = GetComponent<CharacterClass>();
        
        if (characterClass.GlobalCooldown())
        {
            abilityCooldown = characterClass.AbilityCooldown();
        }
    }

    public virtual void UseAbility()
    {

    }

    public virtual IEnumerator TwinSpellCoroutine(CharacterClass character, AbilityClass ability)
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
}