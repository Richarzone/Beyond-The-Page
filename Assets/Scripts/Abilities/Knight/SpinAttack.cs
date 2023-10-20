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
        // Lock the use of abilities
        lockSkill = true;
        characterClass.BlockRotation = true;
        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;
        characterClass.BlockDodge = true;

        characterClass.GetAnimator().SetTrigger("Spin Attack");

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(spinAttackVFX, characterClass.GetVFXPivot().position, spinAttackVFX.transform.rotation);
        vfxSpinInstance.transform.parent = characterClass.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        characterClass.AbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(spinAttackDuration);

        // End the animation of the ability
        characterClass.BlockRotation = false;
        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;
        characterClass.BlockDodge = false;

        characterClass.GetAnimator().SetTrigger("Spin Exit");

        yield return new WaitForSeconds(abilityCooldown);

        lockSkill = false;
    }

    public override IEnumerator TwinSpellCoroutine(CharacterClass character, AbilityClass ability)
    {
        // Lock the use of abilities
        character.BlockAttack = true;
        character.BlockRotation = true;
        character.BlockClassChange = true;
        character.BlockAbilities = true;
        character.BlockDodge = true;

        GameObject twinSpellInstance = Instantiate(twinSpellObject, character.GetVFXPivot().position, twinSpellObject.transform.rotation);
        twinSpellInstance.transform.parent = character.GetVFXPivot();
        
        TickDamageAOE damageComponent = twinSpellInstance.GetComponent<TickDamageAOE>();
        damageComponent.SetDamage(spinAttackDamage);
        damageComponent.SetApplyDamage(spinAttackDamage);
        damageComponent.SetRange(spinAttackRange);

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(spinAttackVFX, character.GetVFXPivot().position, spinAttackVFX.transform.rotation);
        vfxSpinInstance.transform.parent = character.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(spinAttackDuration);

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
                hitCollider.GetComponent<EnemyClass>().Damage(spinAttackDamage, spinAttackDamage);
            }
        }
    }
}