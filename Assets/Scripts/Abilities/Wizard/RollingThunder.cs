using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingThunder : AbilityClass
{
    [SerializeField] private Transform firePivot;

    [Header("Rolling Thunder")]
    [SerializeField] private GameObject thunderPrefab;
    [SerializeField] private float thunderDamage;
    [SerializeField] private float thunderVelocity;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private float abilityRange;
    [SerializeField] private float abilityWidth;

    private Vector3 targetPosition;
    private Vector3 direction;

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

        abilityCanvas.enabled = true;
        abilityCanvas.transform.localScale = new Vector3(abilityWidth, 0 , abilityRange);
        abilityCanvas.transform.localPosition = new Vector3(0, abilityCanvas.transform.localPosition.y, abilityRange / 2f);

        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;

        Cursor.visible = false;
        
        while (skillInput)
        {
            GetMousePosition();

            Quaternion canvasRotation = Quaternion.LookRotation(targetPosition - transform.position);
            canvasRotation.eulerAngles = new Vector3(0f, canvasRotation.eulerAngles.y, 0f);

            abilityCanvas.transform.rotation = Quaternion.Lerp(canvasRotation, abilityCanvas.transform.rotation, 0f);

            yield return null;
        }

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }

        SetDirection();
        InstantiateProjectile(thunderPrefab, firePivot, thunderVelocity, direction, thunderDamage);

        abilityCanvas.enabled = false;

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

    public override IEnumerator TwinSpellCoroutine(CharacterClass character, TwinSpell ability)
    {
        abilityCanvas.enabled = true;

        character.BlockClassChange = true;
        character.BlockAbilities = true;

        Cursor.visible = false;

        while (ability.GetSkillInput())
        {
            GetMousePosition();

            Quaternion canvasRotation = Quaternion.LookRotation(targetPosition - transform.position);
            canvasRotation.eulerAngles = new Vector3(0f, canvasRotation.eulerAngles.y, 0f);

            abilityCanvas.transform.rotation = Quaternion.Lerp(canvasRotation, abilityCanvas.transform.rotation, 0f);

            yield return null;
        }

        SetDirection();
        InstantiateProjectile(thunderPrefab, firePivot, thunderVelocity, direction, thunderDamage);

        abilityCanvas.enabled = false;

        character.BlockClassChange = false;
        character.BlockAbilities = false;

        Cursor.visible = true;
    }

    #region Helpe Function
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

    // Instantiate projectiles
    public void InstantiateProjectile(GameObject projectilePrefab, Transform pivot, float projectileVelocity, Vector3 direction, float damage)
    {
        GameObject instance = Instantiate(projectilePrefab, pivot.position, Quaternion.identity);
        instance.transform.LookAt(targetPosition, Vector3.up);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
        instance.GetComponent<Projectile>().SetDamage(damage, damage);
    }

    // Set projectile direction
    private void SetDirection()
    {
        // Get the direction for the projectile to fly to
        direction = (new Vector3(targetPosition.x, firePivot.position.y, targetPosition.z) - firePivot.position).normalized;
    }
    #endregion
}