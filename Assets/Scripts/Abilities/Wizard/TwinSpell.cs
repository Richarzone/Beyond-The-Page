using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSpell : AbilityClass
{
    [SerializeField] private Transform vfxPivot;

    [Header("Twin Spell")]
    [SerializeField] private ParticleSystem twinSpellVFX;
    [SerializeField] private float nullTwinSpellBuffer;

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

    // Add vfx for null twin spell
    private IEnumerator AbilityCoroutine()
    {
        if (characterClass.AbilityManager().LastUsedSkill != null || characterClass.AbilityManager().LastUsedSkill == this)
        {
            lockSkill = true;

            characterClass.AbilityManager().AbilityCoroutineManager(characterClass.AbilityManager().LastUsedSkill.TwinSpellCoroutine(characterClass, this));

            ParticleSystem vfxTwinSpellInstance = Instantiate(twinSpellVFX, characterClass.GetVFXPivot().position, twinSpellVFX.transform.rotation);
            vfxTwinSpellInstance.transform.parent = characterClass.GetVFXPivot();
            Destroy(vfxTwinSpellInstance.gameObject, vfxTwinSpellInstance.main.duration + vfxTwinSpellInstance.main.startLifetime.constant);

            yield return new WaitForSeconds(abilityCooldown);

            lockSkill = false;
        }
        else
        {
            lockSkill = true;

            yield return new WaitForSeconds(nullTwinSpellBuffer);

            lockSkill = false;
        }
    }
}