using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TwinSpell : AbilityClass
{
    [SerializeField] private Transform vfxPivot;

    [Header("Twin Spell")]
    [SerializeField] private ParticleSystem twinSpellVFX;
    [SerializeField] private float nullTwinSpellBuffer;
    private bool skillLock = false;

   /* protected override void Start()
    {
        base.Start();
        //characterClass.GetAbilityManager().LastUsedSkill = this;
    }*/

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

    // Add vfx for null twin spell
    private IEnumerator AbilityCoroutine()
    {
        if (characterClass.GetAbilityManager().LastUsedSkill == null || characterClass.GetAbilityManager().LastUsedSkill == this)
        {
            blockSkill = true;
            characterClass.GetAnimator().SetTrigger("Twin Spell");

            ParticleSystem vfxTwinSpellInstance = Instantiate(twinSpellVFX, characterClass.GetVFXPivot().position, twinSpellVFX.transform.rotation);
            vfxTwinSpellInstance.transform.parent = characterClass.GetVFXPivot();
            Destroy(vfxTwinSpellInstance.gameObject, vfxTwinSpellInstance.main.duration + vfxTwinSpellInstance.main.startLifetime.constant);

            if (characterClass.GetAbilityManager().BlockAbilitySlots())
            {
                characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
            }

            characterClass.GetAnimator().SetTrigger("Twin Spell Exit");

            yield return new WaitForSeconds(nullTwinSpellBuffer);

            if (characterClass.GetAbilityManager().BlockAbilitySlots())
            {
                characterClass.GetAbilityManager().GetPlayerController().UnlockSkill(skillButton);
            }

            blockSkill = false;
        }
        else
        {
            blockSkill = true;
            characterClass.GetAnimator().SetTrigger("Twin Spell");

            characterClass.GetAbilityManager().AbilityCoroutineManager(characterClass.GetAbilityManager().LastUsedSkill.TwinSpellCoroutine(characterClass, this));

            ParticleSystem vfxTwinSpellInstance = Instantiate(twinSpellVFX, characterClass.GetVFXPivot().position, twinSpellVFX.transform.rotation);
            vfxTwinSpellInstance.transform.parent = characterClass.GetVFXPivot();
            Destroy(vfxTwinSpellInstance.gameObject, vfxTwinSpellInstance.main.duration + vfxTwinSpellInstance.main.startLifetime.constant);

            // Return null until skill used
            while (skillLock)
            {
                yield return null;
            }

            characterClass.GetAnimator().SetTrigger("Twin Spell Exit");

            if (characterClass.GetAbilityManager().BlockAbilitySlots())
            {
                characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
            }

            characterClass.GetAbilityManager().LastUsedSkill = this;

            CallCooldown();

            yield return new WaitForSeconds(abilityCooldown);

            if (characterClass.GetAbilityManager().BlockAbilitySlots())
            {
                characterClass.GetAbilityManager().GetPlayerController().UnlockSkill(skillButton);
            }

            blockSkill = false;
        }
    }

    public void SkillLock()
    {
        skillLock = !skillLock;
    }   
}