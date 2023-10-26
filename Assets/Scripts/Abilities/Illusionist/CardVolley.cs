using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardVolley : AbilityClass
{
    [SerializeField] private Transform firePivot;

    [Header("Card Volley")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private ParticleSystem cardVolleyVFX;
    [SerializeField] private float cardVolleyRange;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileVelocity;
    [SerializeField] private float rateOfFire;
    [SerializeField] private float abilityDuration;
    [SerializeField] private int ammunition;

    [Header("UI")]
    [SerializeField] private Image abilityRangeIndicator;

    [SerializeField] private Transform target;
    private float timeBetweenAttacks;
    [SerializeField] private float currentDuration;
    private int currentAmmo;

    public override void UseAbility()
    {
        currentAmmo = ammunition;
        currentDuration = abilityDuration;

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

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }

        abilityRangeIndicator.gameObject.transform.localScale = (Vector3.one * cardVolleyRange) * 2;
        abilityRangeIndicator.enabled = true;

        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;

        Cursor.visible = false;

        //ParticleSystem hexInstance = 
        
        while (currentAmmo > 0 && currentDuration > 0)
        {
            if (target == null)
            {
                // -------------Look for lowest health enemy-----------
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, cardVolleyRange);

                foreach (Collider collider in hitColliders)
                {
                    if (hitColliders.Length != 0 && (1 << collider.gameObject.layer) == characterClass.GetEnemyLayer().value)
                    {
                        target = collider.transform;
                        break;
                    }
                }
            }
            else
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance <= cardVolleyRange && target != null)
                {
                    Shoot();
                }
                else
                {
                    target = null;
                }
            }

            currentDuration -= Time.deltaTime;

            yield return null;
        }

        //Destroy(hexInstance.gameObject);

        abilityRangeIndicator.enabled = false;

        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;

        Cursor.visible = true;

        characterClass.GetAbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(abilityCooldown);

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().UnlockSkill(skillButton);
        }

        blockSkill = false;
    }

    private void Shoot()
    {
        if (timeBetweenAttacks <= 0f)
        {
            //SetDirection();
            InstantiateHomingProjectile(cardPrefab, firePivot, projectileVelocity, projectileDamage);

            timeBetweenAttacks = 1 / rateOfFire;

            currentAmmo--;
        }

        timeBetweenAttacks -= Time.deltaTime;
    }

    public void InstantiateHomingProjectile(GameObject projectilePrefab, Transform pivot, float projectileVelocity, float damage)
    {
        GameObject instance = Instantiate(projectilePrefab, pivot.position, Quaternion.identity);
        instance.transform.LookAt(target, Vector3.up);

        HomingProjectile instanceProjectile = instance.GetComponent<HomingProjectile>();
        instanceProjectile.SetDamage(damage, damage);
        instanceProjectile.SetVelocity(projectileVelocity);
        instanceProjectile.SetOrigin(firePivot);
        instanceProjectile.SetTarget(target);
    }

    void OnDrawGizmos()
    {
        Color white = Color.white;
        white.a = 0.5f;
        Gizmos.color = white;
        Gizmos.DrawSphere(transform.parent.position, cardVolleyRange);
    }
}