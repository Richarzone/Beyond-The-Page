using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatTrick : AbilityClass
{
    [SerializeField] private Transform firePivot;

    [Header("Hat Trick")]
    [SerializeField] private GameObject hatPrefab;
    [SerializeField] private GameObject characterHat;
    [SerializeField] private ParticleSystem hatTrickTeleportVFX;
    [SerializeField] private float thunderDamage;
    [SerializeField] private float hatVelocity;
    [SerializeField] private float hatRange;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private float abilityWidth;

    private Vector3 targetPosition;
    private Vector3 direction;
    private HatTrickProjectile hatInstance;
    private HatTrickProjectile hatTwinInstance;

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
        characterClass.GetAnimator().SetTrigger("Hat Trick");

        abilityCanvas.enabled = true;
        abilityCanvas.transform.localScale = new Vector3(abilityWidth, 0, hatRange);
        abilityCanvas.transform.localPosition = new Vector3(0, abilityCanvas.transform.localPosition.y, hatRange / 2f);

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

        characterHat.SetActive(false);
        characterClass.GetAnimator().SetTrigger("Hat Trick Exit");

        SetDirection();
        InstantiateProjectile(hatPrefab, firePivot, hatVelocity, direction, thunderDamage);

        abilityCanvas.enabled = false;

        characterClass.BlockClassChange = false;
        characterClass.BlockAbilities = false;

        Cursor.visible = true;

        characterClass.GetAbilityManager().LastUsedSkill = this;

        while (!hatInstance.Arrived())
        {
            yield return null;
        }

        while (hatInstance.Arrived())
        {
            if (skillInput)
            {
                characterClass.GetAbilityManager().gameObject.transform.parent.position = new Vector3(hatInstance.gameObject.transform.position.x, 0, hatInstance.gameObject.transform.position.z);
                
                // Instantiate VFX
                ParticleSystem vfxSpinInstance = Instantiate(hatTrickTeleportVFX, characterClass.GetVFXPivot().position, hatTrickTeleportVFX.transform.rotation);
                Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);
            }

            yield return null;
        }

        characterHat.SetActive(true);

        if (characterClass.GetAbilityManager().BlockAbilitySlots())
        {
            characterClass.GetAbilityManager().GetPlayerController().LockSkill(skillButton);
        }

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

        abilityCanvas.enabled = true;
        abilityCanvas.transform.localScale = new Vector3(abilityWidth, 0, hatRange);
        abilityCanvas.transform.localPosition = new Vector3(0, abilityCanvas.transform.localPosition.y, hatRange / 2f);

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
        InstantiateTwinProjectile(hatPrefab, firePivot, hatVelocity, direction, thunderDamage);

        abilityCanvas.enabled = false;

        character.BlockClassChange = false;
        character.BlockAbilities = false;

        Cursor.visible = true;

        while (!hatTwinInstance.Arrived())
        {
            yield return null;
        }

        while (hatTwinInstance.Arrived())
        {
            if (ability.GetSkillInput())
            {
                characterClass.GetAbilityManager().gameObject.transform.parent.position = new Vector3(hatTwinInstance.gameObject.transform.position.x, 0, hatTwinInstance.gameObject.transform.position.z);

                // Instantiate VFX
                ParticleSystem vfxSpinInstance = Instantiate(hatTrickTeleportVFX, characterClass.GetVFXPivot().position, hatTrickTeleportVFX.transform.rotation);
                Destroy(vfxSpinInstance.gameObject, vfxSpinInstance.main.duration + vfxSpinInstance.main.startLifetime.constant);
            }

            yield return null;
        }
        
        ability.SkillLock();
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

    // Instantiate projectiles
    public void InstantiateProjectile(GameObject projectilePrefab, Transform pivot, float projectileVelocity, Vector3 direction, float damage)
    {
        GameObject instance = Instantiate(projectilePrefab, pivot.position, Quaternion.identity);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
        hatInstance = instance.GetComponent<HatTrickProjectile>();
        hatInstance.SetMaxDistance(hatRange - firePivot.transform.localPosition.z);
    }

    public void InstantiateTwinProjectile(GameObject projectilePrefab, Transform pivot, float projectileVelocity, Vector3 direction, float damage)
    {
        GameObject instance = Instantiate(projectilePrefab, pivot.position, Quaternion.identity);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
        hatTwinInstance = instance.GetComponent<HatTrickProjectile>();
        hatTwinInstance.SetMaxDistance(hatRange - firePivot.transform.localPosition.z);
    }

    // Set projectile direction
    private void SetDirection()
    {
        // Get the direction for the projectile to fly to
        direction = (new Vector3(targetPosition.x, firePivot.position.y, targetPosition.z) - firePivot.position).normalized;
    }
    #endregion
}