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
    [SerializeField] private float thunderRange;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private Image abilityRangeIndicator;
    [SerializeField] private float abilityRange;

    private Vector3 targetPosition;
    private Vector3 habilityTargetPosition;
    private Vector3 direction;
    private float targetRotation;

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
        lockSkill = true;

        Cursor.visible = false;
        
        while (skillInput)
        {
            GetMousePosition();

            Vector3 hitPositionDirection = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(targetPosition, transform.position);
            distance = Mathf.Min(distance, thunderRange / 2f);
            Vector3 newHitPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(newHitPosition.x, 0.01f, newHitPosition.z);

            yield return null;
        }

        SetDirection();
        InstantiateProjectile(thunderPrefab, firePivot, thunderVelocity, direction, thunderDamage);

        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;

        Cursor.visible = true;

        yield return new WaitForSeconds(abilityCooldown);

        lockSkill = false;
    }

    public override IEnumerator TwinSpellCoroutine(CharacterClass character, AbilityClass ability)
    {
        Cursor.visible = false;

        while (ability.GetSkillInput())
        {
            GetMousePosition();

            Vector3 hitPositionDirection = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(targetPosition, transform.position);
            distance = Mathf.Min(distance, thunderRange / 2f);
            Vector3 newHitPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(newHitPosition.x, 0.01f, newHitPosition.z);

            yield return null;
        }

        SetDirection();
        InstantiateProjectile(thunderPrefab, firePivot, thunderVelocity, direction, thunderDamage);

        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;

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
        targetRotation = Mathf.Atan2(targetPosition.y - firePivot.position.y,
                                     targetPosition.x - firePivot.position.x) * Mathf.Rad2Deg;

        direction = (new Vector3(targetPosition.x, firePivot.position.y, targetPosition.z) - firePivot.position).normalized;
    }
    #endregion
}