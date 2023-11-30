using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concoction : AbilityClass
{
    [SerializeField] private GameObject concoctionPrefab;
    [SerializeField] private float damageBuffMultiplier;
    [SerializeField] private float buffDuration;

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
        characterClass.GetAnimator().SetTrigger("Concoction");

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }

        GameObject instance = Instantiate(concoctionPrefab, characterClass.GetVFXPivot().position, concoctionPrefab.transform.rotation);

        ConcoctionBuff buffEfect = instance.GetComponent<ConcoctionBuff>();
        buffEfect.SetDamageValue(damageBuffMultiplier);
        buffEfect.SetBuffDuration(buffDuration);

        instance.transform.parent = characterClass.GetVFXPivot();

        characterClass.GetAbilityManager().LastUsedSkill = this;

        CallCooldown();

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
        
        ConcoctionBuff buffEfect = twinSpellInstance.GetComponent<ConcoctionBuff>();
        buffEfect.SetDamageValue(damageBuffMultiplier);
        buffEfect.SetBuffDuration(buffDuration);

        twinSpellInstance.transform.parent = characterClass.GetVFXPivot();

        yield return null;
    }
}