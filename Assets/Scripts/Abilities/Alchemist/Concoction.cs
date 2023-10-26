using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concoction : AbilityClass
{
    [SerializeField] private GameObject concoctionPrefab;

    public override void UseAbility()
    {
        if (activeSkill && !blockSkill && !characterClass.BlockAbilities)
        {
            characterClass.GetAbilityManager().AbilityCoroutineManager(AbilityCoroutine());
        }
        else
        {
            activeSkill = false;
        }
    }

    private IEnumerator AbilityCoroutine()
    {
        blockSkill = true;

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }

        GameObject twinSpellInstance = Instantiate(concoctionPrefab, characterClass.GetVFXPivot().position, concoctionPrefab.transform.rotation);
        twinSpellInstance.transform.parent = characterClass.GetVFXPivot();

        characterClass.GetAbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(abilityCooldown);

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().UnlockSkill(skillButton);
        }

        blockSkill = false;
    }

    public override IEnumerator TwinSpellCoroutine(CharacterClass character, TwinSpell ability)
    {
        GameObject twinSpellInstance = Instantiate(concoctionPrefab, characterClass.GetVFXPivot().position, concoctionPrefab.transform.rotation);
        twinSpellInstance.transform.parent = characterClass.GetVFXPivot();

        yield return null;
    }
}