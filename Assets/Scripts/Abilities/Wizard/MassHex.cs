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
    private Vector3 abilityTargetPosition;

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
            abilityTargetPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(abilityTargetPosition.x, 0.01f, abilityTargetPosition.z);

            yield return null;
        }

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }
        
        ParticleSystem hexInstance = InstantiateOnPoint(hexPrefab, abilityTargetPosition);
        Destroy(hexInstance.gameObject, hexInstance.main.duration + hexInstance.main.startLifetime.constant);

        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;

        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;

        Cursor.visible = true;

        characterClass.GetAbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(hexBuffer);

        ApplyHex(hexInstance.gameObject);

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
            abilityTargetPosition = transform.position + hitPositionDirection * distance;

            abilityCanvas.transform.position = new Vector3(abilityTargetPosition.x, 0.01f, abilityTargetPosition.z);

            yield return null;
        }

        ParticleSystem hexInstance = InstantiateOnPoint(hexPrefab, abilityTargetPosition);
        Destroy(hexInstance.gameObject, hexInstance.main.duration + hexInstance.main.startLifetime.constant);
        
        ability.SkillLock();

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
    #endregion
}