using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardClass : CharacterClass
{
    [SerializeField] private Transform firePivot;
    [SerializeField] private GameObject basicAttackProjectile;
    [SerializeField] private float projectileVelocity;

    [Header("VFX")]
    [SerializeField] private ParticleSystem teleportVFX;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject weaponVisual;

    private Vector3 targetPosition;
    private Vector3 direction;
    private float targetRotation;

    private void Update()
    {
        GetMousePosition();

        // Attacks
        if (attack && !blockAttack)
        {
            Attack();
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        // If the time between attacks is zero or less than zero return
        if (timeBetweenAttacks > 0f)
        {
            // Refresh the time between attacks
            timeBetweenAttacks -= Time.deltaTime;
        }
    }

    protected override void Attack()
    {
        animator.SetBool("Attack", true);
        
        if (timeBetweenAttacks <= 0f)
        {
            SetDirection();
            InstantiateProjectile(basicAttackProjectile, firePivot, projectileVelocity, direction, attackDamage);
            
            timeBetweenAttacks = 1 / attackSpeed;
        }
    }

    protected override IEnumerator Dodge(float dodgeDuration)
    {
        ParticleSystem vfxEnterInstance = Instantiate(teleportVFX, vfxPivot.position, teleportVFX.transform.rotation);
        Destroy(vfxEnterInstance.gameObject, vfxEnterInstance.main.duration);
        playerVisual.SetActive(false);
        weaponVisual.SetActive(false);
        blockClassChange = true;

        yield return new WaitForSeconds(dodgeDuration);

        ParticleSystem vfxExitInstance = Instantiate(teleportVFX, vfxPivot.position, teleportVFX.transform.rotation);
        Destroy(vfxExitInstance.gameObject, vfxExitInstance.main.duration);
        playerVisual.SetActive(true);
        weaponVisual.SetActive(true);
        blockClassChange = false;
    }

    #region Helper Functions
    private void GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, groundLayer))
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