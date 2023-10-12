using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concoction : AbilityClass
{
    [SerializeField] private GameObject concoctionPrefab;

    public override void UseAbility()
    {
        if (activeSkill && !lockSkill && !characterClass.BlockAbilities)
        {
            characterClass.AbilityManager().AbilityCoroutineManager(AbilityCoroutine());
        }
        else
        {
            activeSkill = false;
        }
    }

    private IEnumerator AbilityCoroutine()
    {
        lockSkill = true;

        GameObject twinSpellInstance = Instantiate(concoctionPrefab, characterClass.GetVFXPivot().position, concoctionPrefab.transform.rotation);
        twinSpellInstance.transform.parent = characterClass.GetVFXPivot();

        yield return new WaitForSeconds(abilityCooldown);

        lockSkill = false;
    }

    public override IEnumerator TwinSpellCoroutine(CharacterClass character, AbilityClass ability)
    {
        GameObject twinSpellInstance = Instantiate(concoctionPrefab, characterClass.GetVFXPivot().position, concoctionPrefab.transform.rotation);
        twinSpellInstance.transform.parent = characterClass.GetVFXPivot();

        yield return null;
    }
}