using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SparklingPowder : AbilityClass
{
    [SerializeField] private Transform firePivot;

    [Header("Stun")]
    [SerializeField] private GameObject stunPrefab;
    [SerializeField] private float stunVelocity;
    [SerializeField] private float stunRange;
    [SerializeField] private float stunAOE;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private Image abilityRangeIndicator;

    private Vector3 targetPosition;
    private Vector3 direction;
    private float targetRotation;

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
        blockSkill = true;

        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(stunRange, stunRange, stunRange);
        abilityCanvas.transform.localScale = (Vector3.one * stunAOE) * 2;
        abilityCanvas.enabled = true;
        abilityRangeIndicator.enabled = true;

        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;

        Cursor.visible = false;

        while (skillInput)
        {
            GetMousePosition();

            Vector3 hitPositionDirection = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(targetPosition, transform.position);
            distance = Mathf.Min(distance, stunRange / 2f);
            Vector3 newHitPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(newHitPosition.x, 1f, newHitPosition.z);

            yield return null;
        }

        characterClass.GetAnimator().SetTrigger("Stun");

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }

        SetDirectionTarget();
        InstantiateProjectileTarget(stunPrefab, firePivot, stunVelocity, direction);

        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;

        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;

        Cursor.visible = true;

        characterClass.GetAbilityManager().LastUsedSkill = this;

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

        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(stunRange, stunRange, stunRange);
        abilityCanvas.transform.localScale = (Vector3.one * stunAOE) * 2;
        abilityCanvas.enabled = true;
        abilityRangeIndicator.enabled = true;

        character.BlockClassChange = true;
        character.BlockAbilities = true;

        Cursor.visible = false;

        while (ability.GetSkillInput())
        {
            GetMousePosition();

            Vector3 hitPositionDirection = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(targetPosition, transform.position);
            distance = Mathf.Min(distance, stunRange / 2f);
            Vector3 newHitPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(newHitPosition.x, 1f, newHitPosition.z);

            yield return null;
        }

        ability.SkillLock();

        SetDirectionTarget();
        InstantiateProjectileTarget(stunPrefab, firePivot, stunVelocity, direction);

        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;

        character.BlockClassChange = false;
        character.BlockAbilities = false;

        Cursor.visible = true;
    }

    #region Helper Functions
    private void GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, characterClass.GetGroundLayer()))
        {
            if (raycastHit.collider.gameObject != gameObject.transform.parent)
            {
                targetPosition = raycastHit.point;
            }
        }
    }

    // Instantiate projectiles to travel to specific point
    private void InstantiateProjectileTarget(GameObject projectilePrefab, Transform pivot, float projectileVelocity, Vector3 direction)
    {
        GameObject instance = Instantiate(projectilePrefab, new Vector3(pivot.position.x, pivot.position.y + 0.5f, pivot.position.z), Quaternion.identity);
        Projectile projectileInstance = instance.GetComponent<Projectile>();
        projectileInstance.SetAOERange(stunAOE);
        projectileInstance.SetInstanceOwner(characterClass);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
    }

    // Set projectile specific targeted direction
    private void SetDirectionTarget()
    {
        Vector3 hitPositionDirection = (targetPosition - firePivot.position).normalized;
        float distance = Vector3.Distance(targetPosition, transform.position);
        distance = Mathf.Min(distance, stunRange / 2f);
        Vector3 newHitPosition = firePivot.position + hitPositionDirection * distance;

        direction = (newHitPosition - firePivot.position).normalized;
    }
    #endregion
}