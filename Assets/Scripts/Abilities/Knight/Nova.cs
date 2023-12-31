using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nova : AbilityClass
{
    [Header("Nova")]
    [SerializeField] private ParticleSystem novaVFX;
    [SerializeField] private GameObject twinSpellObject;
    [SerializeField] private float novaNormalDamage;
    [SerializeField] private float novaCriticalDamage;
    [SerializeField] private float novaAttackRange;
    [SerializeField] private float novaCriticalRange;
    [SerializeField] private float novaPushForce;
    [SerializeField] private float novaRiseForce;
    [SerializeField] private float novaBuffer;
    [SerializeField] private float novaDownTime;

    [Header("UI")]
    [SerializeField] private Image abilityRangeIndicator;

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
        // Lock the use of the abilities
        lockSkill = true;

        abilityRangeIndicator.gameObject.transform.localScale = (Vector3.one * novaAttackRange) * 2;
        abilityRangeIndicator.enabled = true;

        Cursor.visible = false;

        while (skillInput)
        {
            yield return null;
        }

        characterClass.BlockMovement = true;
        characterClass.BlockRotation = true;
        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;
        characterClass.BlockDodge = true;

        abilityRangeIndicator.enabled = false;

        // Start the animation of the ability
        characterClass.GetAnimator().SetTrigger("Nova");

        Cursor.visible = true;

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(novaVFX, characterClass.GetVFXPivot().position, novaVFX.transform.rotation);
        vfxSpinInstance.transform.parent = characterClass.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(novaBuffer);

        NovaDamage();

        yield return new WaitForSeconds(novaDownTime);

        // End the animation of the ability
        characterClass.BlockMovement = false;
        characterClass.BlockRotation = false;
        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;
        characterClass.BlockDodge = false;

        yield return new WaitForSeconds(abilityCooldown);

        lockSkill = false;
    }

    public override IEnumerator TwinSpellCoroutine(CharacterClass character, AbilityClass ability)
    {
        abilityRangeIndicator.gameObject.transform.localScale = (Vector3.one * novaAttackRange) * 2;
        abilityRangeIndicator.enabled = true;

        Cursor.visible = false;

        while (ability.GetSkillInput())
        {
            yield return null;
        }

        character.BlockMovement = true;
        character.BlockRotation = true;
        character.BlockAbilities = true;
        character.BlockClassChange = true;

        abilityRangeIndicator.enabled = false;

        Cursor.visible = true;

        GameObject twinSpellInstance = Instantiate(twinSpellObject, character.GetVFXPivot().position, twinSpellObject.transform.rotation);
        twinSpellInstance.transform.parent = character.GetVFXPivot();
        twinSpellInstance.transform.position = Vector3.zero;

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(novaVFX, character.GetVFXPivot().position, novaVFX.transform.rotation);
        vfxSpinInstance.transform.parent = character.GetVFXPivot();
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(novaBuffer);

        NovaDamage();

        yield return new WaitForSeconds(novaDownTime);

        // End the animation of the ability
        character.BlockMovement = false;
        character.BlockRotation = false;
        character.BlockAbilities = false;
        character.BlockClassChange = false;
    }

    // Damage Event
    private void NovaDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, novaAttackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if ((1 << hitCollider.gameObject.layer) == characterClass.GetEnemyLayer().value)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                hitCollider.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x * novaPushForce, direction.y + novaRiseForce, direction.z * novaPushForce), ForceMode.Force);

                if (distance <= novaCriticalRange)
                {
                    hitCollider.GetComponent<EnemyClass>().Damage(novaCriticalDamage, novaNormalDamage);
                }
                else if (distance >= novaCriticalRange)
                {
                    hitCollider.GetComponent<EnemyClass>().Damage(novaNormalDamage, novaNormalDamage);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Color white = Color.white;
        white.a = 0.5f;
        Gizmos.color = white;
        Gizmos.DrawSphere(transform.parent.position, novaAttackRange);
    }
}