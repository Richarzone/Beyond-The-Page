using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightClass : CharacterClass
{
    [SerializeField] private Transform vfxPivot;

    [Header("Habilities")]
    [SerializeField] private float spinAttackDuration;

    [Header("VFX")]
    [SerializeField] private ParticleSystem spinAttackVFX;
    [SerializeField] private float spinRange;
    [SerializeField] private ParticleSystem swirlVFX;
    [SerializeField] private float swirlRange;
    [SerializeField] private ParticleSystem novaVFX;
    [SerializeField] private float novaRange;

    [Header("UI")]
    [SerializeField] private Image abilityRangeIndicator;

    private void Start()
    {
        
    }

    private void Update()
    {
        // Check the input for basic attack attack
        if (attack && !lockAttack)
        {
            Attack();
        }

        // Check inputs for skills
        if (activeSkill1 && !lockSkill1)
        {
            StartCoroutine(Skill1());
        }
        else
        {
            activeSkill1 = false;
        }

        if (activeSkill2 && !lockSkill2)
        {
            AimSkill2();
        }
        else
        {
            activeSkill2 = false;
        }

        if (activeSkill3 && !lockSkill3)
        {
            AimSkill3();
        }
        else
        {
            activeSkill3 = false;
        }

        // If the time between attacks is zero or less than zero return
        if (timeBetweenAttacks <= 0f)
        {
            return;
        }

        // Refresh the time between attacks
        timeBetweenAttacks -= Time.deltaTime;
    }

    protected override void Attack()
    {
        if (timeBetweenAttacks <= 0f)
        {
            animator.SetTrigger("Attack");
            timeBetweenAttacks = 1 / attackSpeed;
        }
    }

    #region Skills
    private void AimSkill2()
    {
        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(swirlRange, swirlRange, swirlRange);
        abilityRangeIndicator.enabled = true;

        Cursor.visible = false;

        if (!skillInput2)
        {
            activeSkill2 = false;
            StartCoroutine(Skill2());

            abilityRangeIndicator.enabled = false;

            Cursor.visible = true;
        }
    }

    private void AimSkill3()
    {
        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(novaRange, novaRange, novaRange);
        abilityRangeIndicator.enabled = true;

        Cursor.visible = false;

        if (!skillInput3)
        {
            activeSkill3 = false;
            StartCoroutine(Skill3());

            abilityRangeIndicator.enabled = false;

            Cursor.visible = true;
        }
    }

    protected override IEnumerator Skill1()
    {
        // Remove attack lock to buffer attack

        // Lock the use of the hability and attacking
        lockSkill1 = true;
        lockAttack = true;
        blockRotation = true;
        animator.SetTrigger("Spin Attack");

        // Instantiate VFX
        ParticleSystem vfxSpinInstance = Instantiate(spinAttackVFX, vfxPivot.position, spinAttackVFX.transform.rotation);
        vfxSpinInstance.transform.parent = vfxPivot;
        Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(spinAttackDuration);

        // End the animation of the hability
        lockAttack = false;
        blockRotation = false;
        animator.SetTrigger("Spin Exit");

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill1 = false;
    }

    protected override IEnumerator Skill2()
    {
        lockSkill2 = true;
        animator.SetTrigger("Swirl");

        ParticleSystem vfxSwirlInstance = Instantiate(swirlVFX, vfxPivot.position, swirlVFX.transform.rotation);
        vfxSwirlInstance.transform.parent = vfxPivot;
        Destroy(vfxSwirlInstance.gameObject, vfxSwirlInstance.main.duration + vfxSwirlInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill2 = false;
    }

    protected override IEnumerator Skill3()
    {
        lockSkill3 = true;
        animator.SetTrigger("Nova");
        ParticleSystem vfxNovaInstance = Instantiate(novaVFX, vfxPivot.position, novaVFX.transform.rotation);
        Destroy(vfxNovaInstance.gameObject, vfxNovaInstance.main.duration + vfxNovaInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill3 = false;
    }
    #endregion

    #region Events
    private void Damage()
    {
        Debug.Log("Do damage");
    }

    private void EnableMovement()
    {
        blockMovement = false;
    }

    private void DisableMovement()
    {
        blockMovement = true;
    }

    private void EnableRotation()
    {
        blockRotation = false;
    }

    private void DisableRotation()
    {
        blockRotation = true;
    }
    #endregion
}