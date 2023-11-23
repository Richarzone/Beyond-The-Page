using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemistClass : CharacterClass
{
    [SerializeField] private Transform firePivot;
    //[SerializeField] private LayerMask groundLayer;

    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float instanceDelay;
    [SerializeField] private float projectileVelocity;

    [Header("VFX")]
    [SerializeField] private ParticleSystem smokeBombVFX;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject weaponVisual;

    private Vector3 targetPosition;
    private Vector3 direction;
    private Vector3 targetDirection;
    private float targetRotation;

    private void Update()
    {
        // Check the input for basic attack attack
        if (attack && !blockAttack)
        {
            Attack();
        }

        // Get world position of the mouse
        GetMousePosition();

        // If the time between attacks is zero or less than zero return
        if (timeBetweenAttacks > 0f)
        {
            // Refresh the time between attacks
            timeBetweenAttacks -= Time.deltaTime;
        }

        Debug.Log(attackSpeed * AttackSpeed());
    }

    protected override void Attack()
    {
        if (timeBetweenAttacks <= 0f)
        {
            StartCoroutine(DelayAttack());
            timeBetweenAttacks = 1 / (attackSpeed * AttackSpeed());
        }
    }

    protected override IEnumerator Dodge(float dodgeDuration)
    {
        ParticleSystem vfxEnterInstance = Instantiate(smokeBombVFX, vfxPivot.position, smokeBombVFX.transform.rotation);
        Destroy(vfxEnterInstance.gameObject, vfxEnterInstance.main.duration);
        playerVisual.SetActive(false);
        weaponVisual.SetActive(false);
        blockClassChange = true;

        yield return new WaitForSeconds(dodgeDuration);

        ParticleSystem vfxExitInstance = Instantiate(smokeBombVFX, vfxPivot.position, smokeBombVFX.transform.rotation);
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
    private void InstantiateProjectile(GameObject projectilePrefab, Transform pivot, float projectileVelocity, Vector3 direction, float damage)
    {
        GameObject instance = Instantiate(projectilePrefab, pivot.position, Quaternion.identity);
        instance.transform.LookAt(targetPosition, Vector3.up);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
        instance.GetComponent<Projectile>().SetDamage(AttackDamage(), damage);
    }

    // Set projectile direction
    private void SetDirection()
    {
        // Get the direction for the projectile to fly to
        targetRotation = Mathf.Atan2(targetPosition.y - firePivot.position.y,
                                     targetPosition.x - firePivot.position.x) * Mathf.Rad2Deg;

        direction = (new Vector3(targetPosition.x, firePivot.position.y, targetPosition.z) - firePivot.position).normalized;
    }

    // Dalay basic attacks
    private IEnumerator DelayAttack()
    {
        animator.SetBool("Attack", true);
        SetDirection();

        yield return new WaitForSeconds(instanceDelay);

        InstantiateProjectile(projectile, firePivot, projectileVelocity, direction, attackDamage);
    }
    #endregion
}
