using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public partial class ClosingAct : AbilityClass
{
    [SerializeField] private WeaponCollisionDetection weapon;

    [Header("Closing Act")]
    [SerializeField] private GameObject hatPrefab;
    [SerializeField] private ParticleSystem hatTrickTeleportVFX;
    [SerializeField] private float closingActDamage;
    [SerializeField] private float minimumDamage;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    [Header("Hit Box")]
    [SerializeField] private GameObject hitBox;
    [SerializeField] private float hitBoxSize;
    [SerializeField] private float hitBoxLenght;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private float abilityWidth;

    private BoxCollider hitBoxCollider;
    //private ClosingActDamage damageDetection;
    private Vector3 targetPosition;
    private float currentDamage;

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
        abilityCanvas.transform.localScale = new Vector3(abilityWidth, 0, dashSpeed * dashDuration);
        abilityCanvas.transform.localPosition = new Vector3(0, abilityCanvas.transform.localPosition.y, (dashSpeed * dashDuration) / 2f);

        characterClass.BlockClassChange = true;
        characterClass.BlockAbilities = true;

        currentDamage = closingActDamage;

        Cursor.visible = false;

        while (skillInput)
        {
            GetMousePosition();

            Quaternion canvasRotation = Quaternion.LookRotation(targetPosition - transform.position);
            canvasRotation.eulerAngles = new Vector3(0f, canvasRotation.eulerAngles.y, 0f);

            abilityCanvas.transform.rotation = Quaternion.Lerp(canvasRotation, abilityCanvas.transform.rotation, 0f);

            yield return null;
        }

        characterClass.GetAnimator().SetTrigger("Closing Act");

        characterClass.BlockRotation = true;
        characterClass.IsDashing = true;

        ClosingActDamage();

        Rigidbody playerRigidBody = characterClass.GetAbilityManager().GetPlayerController().GetRigidBody();
        playerRigidBody.AddForce(transform.forward.normalized * dashSpeed - playerRigidBody.velocity, ForceMode.VelocityChange);

        characterClass.GetAbilityManager().GetPlayerController().GetPlayerCollider().enabled = false;

        abilityCanvas.enabled = false;

        Cursor.visible = true;

        characterClass.GetAbilityManager().LastUsedSkill = this;

        yield return new WaitForSeconds(dashDuration);

        characterClass.BlockRotation = false;
        characterClass.BlockAbilities = false;
        characterClass.BlockClassChange = false;
        characterClass.IsDashing = false;
        characterClass.GetAbilityManager().GetPlayerController().GetPlayerCollider().enabled = true;

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

        currentDamage = closingActDamage;

        abilityCanvas.enabled = true;
        abilityCanvas.transform.localScale = new Vector3(abilityWidth, 0, dashSpeed * dashDuration);
        abilityCanvas.transform.localPosition = new Vector3(0, abilityCanvas.transform.localPosition.y, (dashSpeed * dashDuration) / 2f);

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

        ability.SkillLock();

        character.BlockRotation = true;

        character.IsDashing = true;

        ClosingActDamage();

        Rigidbody playerRigidBody = characterClass.GetAbilityManager().transform.parent.GetComponent<Rigidbody>();
        playerRigidBody.AddForce(transform.forward.normalized * dashSpeed - playerRigidBody.velocity, ForceMode.VelocityChange);

        characterClass.GetAbilityManager().GetPlayerController().GetPlayerCollider().enabled = false;

        abilityCanvas.enabled = false;

        Cursor.visible = true;

        yield return new WaitForSeconds(dashDuration);

        character.BlockRotation = false;
        character.BlockAbilities = false;
        character.BlockClassChange = false;
        character.IsDashing = false;
        characterClass.GetAbilityManager().GetPlayerController().GetPlayerCollider().enabled = true;
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

    private void ClosingActDamage()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, hitBoxSize, characterClass.GetAbilityManager().transform.parent.forward, hitBoxSize * hitBoxLenght, characterClass.GetEnemyLayer().value);
        hits = hits.OrderBy(hit => hit.distance).ToArray();

        foreach (RaycastHit hit in hits)
        {
            hit.collider.GetComponent<EnemyClass>().Damage(currentDamage, currentDamage);

            if (Mathf.Floor(currentDamage * 0.25f) >= minimumDamage)
            {
                currentDamage = Mathf.Floor(currentDamage * 0.25f);
            }
        }
    }

    void OnDrawGizmos()
    {
        Transform playerTranform = transform.parent.parent.transform;
        Color red = Color.red;
        red.a = 0.5f;
        Gizmos.color = red;
        Gizmos.DrawCube(new Vector3(playerTranform.position.x, playerTranform.position.y + 1, playerTranform.position.z + (hitBoxSize * hitBoxLenght) / 2), 
                        new Vector3(hitBoxSize, hitBoxSize, hitBoxSize * hitBoxLenght));
    }
    #endregion
}