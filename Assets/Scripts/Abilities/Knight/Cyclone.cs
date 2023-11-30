using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cyclone : AbilityClass
{
    [Header("Cyclone")]
    [SerializeField] private ParticleSystem cycloneVFX;
    [SerializeField] private float cycloneAttackDamage;
    [SerializeField] private float cycloneAttackRange;
    [SerializeField] private float cyclonePushForce;
    [SerializeField] private float cycloneRiseForce;
    [SerializeField] private float cycloneBuffer;
    [SerializeField] private float cycloneDownTime;

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

        abilityRangeIndicator.gameObject.transform.localScale = (Vector3.one * cycloneAttackRange) * 2;
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
        characterClass.GetAnimator().SetTrigger("Cyclone");

        Cursor.visible = true;

        characterClass.GetAbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(cycloneBuffer);

        SwirlDamage();

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(cycloneVFX, characterClass.GetVFXPivot().position, cycloneVFX.transform.rotation);
        vfxSpinInstance.transform.parent = characterClass.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(cycloneDownTime);

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

        abilityRangeIndicator.gameObject.transform.localScale = (Vector3.one * cycloneAttackRange) * 2;
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

        yield return new WaitForSeconds(cycloneBuffer);

        SwirlDamage();

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(cycloneVFX, character.GetVFXPivot().position, cycloneVFX.transform.rotation);
        vfxSpinInstance.transform.parent = character.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(cycloneDownTime);

        // End the animation of the ability
        character.BlockClassChange = false;
        character.BlockAbilities = false;
    }

    // Damage Event
    private void SwirlDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, cycloneAttackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if ((1 << hitCollider.gameObject.layer) == characterClass.GetEnemyLayer().value)
            {
                hitCollider.GetComponent<EnemyClass>().CanBeKnocked = true;
                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                //hitCollider.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x * -swirlPushForce, direction.y + swirlRiseForce, direction.z * -swirlPushForce), ForceMode.Force);
                hitCollider.GetComponent<EnemyClass>().Force = new Vector3(direction.x * -cyclonePushForce, direction.y + cycloneRiseForce, direction.z * -cyclonePushForce);
                hitCollider.GetComponent<EnemyClass>().Damage(cycloneAttackDamage +
                                                             (cycloneAttackDamage * characterClass.GetAbilityManager().GetPlayerController().DamageMultiplier()) +
                                                             (cycloneAttackDamage * characterClass.GetConcoctionDamageMultiplier()), cycloneAttackDamage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Color yellow = Color.yellow;
        yellow.a = 0.5f;
        Gizmos.color = yellow;
        Gizmos.DrawSphere(transform.parent.position, cycloneAttackRange);
    }
}
