using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swirl : AbilityClass
{
    [Header("Swirl")]
    [SerializeField] private ParticleSystem swirlVFX;
    [SerializeField] private float swirlAttackDamage;
    [SerializeField] private float swirlAttackRange;
    [SerializeField] private float swirlPushForce;
    [SerializeField] private float swirlRiseForce;
    [SerializeField] private float swirlBuffer;
    [SerializeField] private float swirlDownTime;

    [Header("UI")]
    [SerializeField] private Image abilityRangeIndicator;

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
        // Lock the use of the habilities
        blockSkill = true;

        abilityRangeIndicator.gameObject.transform.localScale = (Vector3.one * swirlAttackRange) * 2;
        abilityRangeIndicator.enabled = true;

        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;

        Cursor.visible = false;

        while (skillInput)
        {
            yield return null;
        }

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }
        
        characterClass.BlockDodge = true;

        abilityRangeIndicator.enabled = false;
        characterClass.GetAnimator().SetTrigger("Swirl");

        Cursor.visible = true;

        characterClass.GetAbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(swirlBuffer);

        SwirlDamage();

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(swirlVFX, characterClass.GetVFXPivot().position, swirlVFX.transform.rotation);
        vfxSpinInstance.transform.parent = characterClass.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(swirlDownTime);

        // End the animation of the ability
        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;
        characterClass.BlockDodge = false;

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
        ability.SkillLock();

        abilityRangeIndicator.gameObject.transform.localScale = (Vector3.one * swirlAttackRange) * 2;
        abilityRangeIndicator.enabled = true;

        character.BlockClassChange = true;
        character.BlockAbilities = true;

        Cursor.visible = false;

        while (ability.GetSkillInput())
        {
            yield return null;
        }

        ability.SkillLock();

        abilityRangeIndicator.enabled = false;

        Cursor.visible = true;

        yield return new WaitForSeconds(swirlBuffer);

        SwirlDamage();

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(swirlVFX, character.GetVFXPivot().position, swirlVFX.transform.rotation);
        vfxSpinInstance.transform.parent = character.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(swirlDownTime);

        // End the animation of the ability
        character.BlockClassChange = false;
        character.BlockAbilities = false;
    }

    // Damage Event
    private void SwirlDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, swirlAttackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if ((1 << hitCollider.gameObject.layer) == characterClass.GetEnemyLayer().value)
            {
                hitCollider.GetComponent<EnemyClass>().CanBeKnocked = true;
                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                //hitCollider.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x * -swirlPushForce, direction.y + swirlRiseForce, direction.z * -swirlPushForce), ForceMode.Force);
                hitCollider.GetComponent<EnemyClass>().Force = new Vector3(direction.x * -swirlPushForce, direction.y + swirlRiseForce, direction.z * -swirlPushForce);
                hitCollider.GetComponent<EnemyClass>().Damage(swirlAttackDamage, swirlAttackDamage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Color yellow = Color.yellow;
        yellow.a = 0.5f;
        Gizmos.color = yellow;
        Gizmos.DrawSphere(transform.parent.position, swirlAttackRange);
    }
}
