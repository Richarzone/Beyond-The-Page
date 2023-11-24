using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : AbilityClass
{
    [Header("Spin Attack")]
    [SerializeField] private ParticleSystem spinAttackVFX;
    [SerializeField] private GameObject twinSpellObject;
    [SerializeField] private float spinAttackDamage;
    [SerializeField] private float spinAttackRange;
    [SerializeField] private float spinAttackDuration;

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
        // Lock the use of abilities
        blockSkill = true;

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }
        
        characterClass.BlockRotation = true;
        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;
        characterClass.BlockDodge = true;


        characterClass.GetAnimator().SetTrigger("Spin Attack");

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(spinAttackVFX, characterClass.GetVFXPivot().position, spinAttackVFX.transform.rotation);
        vfxSpinInstance.transform.parent = characterClass.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        characterClass.GetAbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(spinAttackDuration);

        // End the animation of the ability
        characterClass.BlockRotation = false;
        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;
        characterClass.BlockDodge = false;

        characterClass.GetAnimator().SetTrigger("Spin Exit");

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
        // Lock the use of abilities
        character.BlockAttack = true;
        character.BlockRotation = true;
        character.BlockClassChange = true;
        character.BlockAbilities = true;
        character.BlockDodge = true;

        float currentDuration = spinAttackDuration;

        GameObject twinSpellInstance = Instantiate(twinSpellObject, character.GetVFXPivot().position, twinSpellObject.transform.rotation);
        twinSpellInstance.transform.parent = character.GetVFXPivot();
        
        TickDamageAOE damageComponent = twinSpellInstance.GetComponent<TickDamageAOE>();
        damageComponent.SetDamage(spinAttackDamage);
        damageComponent.SetRange(spinAttackRange);

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(spinAttackVFX, character.GetVFXPivot().position, spinAttackVFX.transform.rotation);
        vfxSpinInstance.transform.parent = character.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        while (currentDuration > 0)
        {
            damageComponent.SetApplyDamage(spinAttackDamage +
                    (spinAttackDamage * characterClass.GetAbilityManager().GetPlayerController().DamageMultiplier()) +
                    (spinAttackDamage * characterClass.GetConcoctionDamageMultiplier()));
            currentDuration -= Time.deltaTime;
            yield return null;
        }

        // End the animation of the ability
        character.BlockAttack = false;
        character.BlockRotation = false;
        character.BlockClassChange = false;
        character.BlockAbilities = false;
        character.BlockDodge = false;

        twinSpellInstance.GetComponent<Animator>().SetTrigger("Spin Exit");
    }

    // Damage Event
    public void SpinDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spinAttackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if ((1 << hitCollider.gameObject.layer) == characterClass.GetEnemyLayer().value)
            {
                hitCollider.GetComponent<EnemyClass>().Damage(spinAttackDamage + 
                                                             (spinAttackDamage * characterClass.GetAbilityManager().GetPlayerController().DamageMultiplier()) +
                                                             (spinAttackDamage * characterClass.GetConcoctionDamageMultiplier()), spinAttackDamage);
            }
        }
    }
}