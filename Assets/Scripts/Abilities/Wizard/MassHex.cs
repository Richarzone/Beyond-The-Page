using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassHex : AbilityClass
{
    [SerializeField] private Transform firePivot;

    [Header("Mass Hex")]
    [SerializeField] private ParticleSystem hexPrefab;
    [SerializeField] private float hexRange;
    [SerializeField] private float hexAOE;
    [SerializeField] private float hexBaseMultiplyer;
    [SerializeField] private float hexMultiplyerIncrement;
    [SerializeField] private float hexDuration;
    [SerializeField] private float hexBuffer;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private Image abilityRangeIndicator;

    private Vector3 targetPosition;
    private Vector3 habilityTargetPosition;
    private Vector3 direction;

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

        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(hexRange, hexRange, hexRange);
        abilityCanvas.transform.localScale = (Vector3.one * hexAOE) * 2;
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
            distance = Mathf.Min(distance, hexRange / 2f);
            habilityTargetPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(habilityTargetPosition.x, 0.01f, habilityTargetPosition.z);

            yield return null;
        }

        SetDirectionTarget();
        ParticleSystem hexInstance = InstantiateOnPoint(hexPrefab, habilityTargetPosition);
        Destroy(hexInstance.gameObject, hexInstance.main.duration + hexInstance.main.startLifetime.constant);

        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;

        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;

        Cursor.visible = true;

        characterClass.AbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(hexBuffer);

        ApplyHex(hexInstance.gameObject);

        yield return new WaitForSeconds(abilityCooldown);

        lockSkill = false;
    }

    public override IEnumerator TwinSpellCoroutine(CharacterClass character, AbilityClass ability)
    {
        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(hexRange, hexRange, hexRange);
        abilityCanvas.transform.localScale = (Vector3.one * hexAOE) * 2;
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
            distance = Mathf.Min(distance, hexRange / 2f);
            habilityTargetPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(habilityTargetPosition.x, 0.01f, habilityTargetPosition.z);

            yield return null;
        }

        SetDirectionTarget();
        ParticleSystem hexInstance = InstantiateOnPoint(hexPrefab, habilityTargetPosition);
        Destroy(hexInstance.gameObject, hexInstance.main.duration + hexInstance.main.startLifetime.constant);

        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;

        character.BlockClassChange = false;
        character.BlockAbilities = false;

        Cursor.visible = true;

        yield return new WaitForSeconds(hexBuffer);

        ApplyHex(hexInstance.gameObject);
    }

    private void ApplyHex(GameObject hexInstance)
    {
        Collider[] hitColliders = Physics.OverlapSphere(hexInstance.transform.position, hexAOE);

        foreach (Collider hitCollider in hitColliders)
        {
            if ((1 << hitCollider.gameObject.layer) == characterClass.GetEnemyLayer().value)
            {
                EnemyClass enemyHit = hitCollider.GetComponent<EnemyClass>();
                enemyHit.ApplyHex(hexBaseMultiplyer, hexMultiplyerIncrement, hexDuration);
            }
        }
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

    // Instantiate object on specific point
    private ParticleSystem InstantiateOnPoint(ParticleSystem projectilePrefab, Vector3 point)
    {
        return Instantiate(projectilePrefab, new Vector3(point.x, point.y + 0.5f, point.z), projectilePrefab.transform.rotation);
    }

    // Set projectile specific targeted direction
    private void SetDirectionTarget()
    {
        Vector3 hitPositionDirection = (targetPosition - firePivot.position).normalized;
        float distance = Vector3.Distance(targetPosition, transform.position);
        distance = Mathf.Min(distance, hexRange / 2f);
        Vector3 newHitPosition = firePivot.position + hitPositionDirection * distance;

        direction = (newHitPosition - firePivot.position).normalized;
    }
    #endregion
}