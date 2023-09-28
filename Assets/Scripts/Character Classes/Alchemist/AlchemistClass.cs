using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemistClass : CharacterClass
{
    [SerializeField] private Transform firePivot;
    [SerializeField] private Transform vfxPivot;
    [SerializeField] private LayerMask groundLayer;

    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float instanceDelay;
    [SerializeField] private float projectileVelocity;

    [Header("Habilities")]
    [SerializeField] private GameObject molotovPrefab;
    [SerializeField] private float molotovVelocity;
    [SerializeField] private float molotovRange;
    [SerializeField] private float molotovAOE;
    [SerializeField] private GameObject stunPrefab;
    [SerializeField] private float stunVelocity;
    [SerializeField] private float stunRange;
    [SerializeField] private float stunAOE;
    [SerializeField] private GameObject concoctionPrefab;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private Image abilityRangeIndicator;
    [SerializeField] private float abilityRange;

    [Header("VFX")]
    [SerializeField] private ParticleSystem smokeBombVFX;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject weaponVisual;

    private Vector3 targetPosition;
    private Vector3 direction;
    private Vector3 targetDirection;
    private float targetRotation;

    private void Start()
    {
        abilityCanvas.enabled = false;
        abilityRangeIndicator.enabled = false;
    }

    private void Update()
    {
        // Check the input for basic attack attack
        if (attack)
        {
            Attack();
        }

        // Get world position of the mouse
        GetMousePosition();

        // Check inputs for skills
        if (activeSkill1 && !lockSkill1)
        {
            AimSkill1();
        }
        else
        {
            activeSkill1 = false;
        }

        if (activeSkill2 && !lockSkill2)
        {
            AimSkill2();
        }
        else
        {
            activeSkill2 = false;
        }

        if (activeSkill3 && !lockSkill3)
        {
            StartCoroutine(Skill3());
        }
        else
        {
            activeSkill3 = false;
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
        if (timeBetweenAttacks <= 0f)
        {
            StartCoroutine(DelayAttack());
            timeBetweenAttacks = 1 / attackSpeed;
        }
    }

    #region Skills
    private void AimSkill1()
    {
        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(molotovRange, molotovRange, molotovRange);
        abilityCanvas.transform.localScale = new Vector3(molotovAOE, molotovAOE, molotovAOE);
        abilityCanvas.enabled = true;
        abilityRangeIndicator.enabled = true;

        Vector3 hitPositionDirection = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(targetPosition, transform.position);
        distance = Mathf.Min(distance, abilityRange);
        Vector3 newHitPosition = transform.position + hitPositionDirection * distance;

        abilityCanvas.transform.position = new Vector3(newHitPosition.x, 0.01f, newHitPosition.z);

        Cursor.visible = false;

        if (!skillInput1)
        {
            activeSkill1 = false;
            StartCoroutine(Skill1());

            abilityCanvas.enabled = false;
            abilityRangeIndicator.enabled = false;

            Cursor.visible = true;
        }
    }

    private void AimSkill2()
    {
        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(stunRange, stunRange, stunRange);
        abilityCanvas.transform.localScale = new Vector3(stunAOE, stunAOE, stunAOE);
        abilityCanvas.enabled = true;
        abilityRangeIndicator.enabled = true;

        Vector3 hitPositionDirection = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(targetPosition, transform.position);
        distance = Mathf.Min(distance, abilityRange);
        Vector3 newHitPosition = transform.position + hitPositionDirection * distance;

        abilityCanvas.transform.position = new Vector3(newHitPosition.x, 0.01f, newHitPosition.z);

        Cursor.visible = false;

        if (!skillInput2)
        {
            activeSkill2 = false;
            StartCoroutine(Skill2());

            abilityCanvas.enabled = false;
            abilityRangeIndicator.enabled = false;

            Cursor.visible = true;
        }
    }

    protected override IEnumerator Skill1()
    {
        lockSkill1 = true;
        SetDirectionTarget();
        InstantiateProjectileTarget(molotovPrefab, firePivot, molotovVelocity, direction);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill1 = false;
    }

    protected override IEnumerator Skill2()
    {
        lockSkill2 = true;
        SetDirectionTarget();
        InstantiateProjectileTarget(stunPrefab, firePivot, stunVelocity, direction);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill2 = false;
    }

    protected override IEnumerator Skill3()
    {
        lockSkill3 = true;
        //activeSkill3 = false;
        Instantiate(concoctionPrefab, vfxPivot.transform);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill3 = false;
    }
    #endregion

    protected override IEnumerator Dodge(float dodgeDuration)
    {
        ParticleSystem vfxEnterInstance = Instantiate(smokeBombVFX, vfxPivot.position, smokeBombVFX.transform.rotation);
        Destroy(vfxEnterInstance.gameObject, vfxEnterInstance.main.duration);
        playerVisual.SetActive(false);
        weaponVisual.SetActive(false);

        yield return new WaitForSeconds(dodgeDuration);

        ParticleSystem vfxExitInstance = Instantiate(smokeBombVFX, vfxPivot.position, smokeBombVFX.transform.rotation);
        Destroy(vfxExitInstance.gameObject, vfxExitInstance.main.duration);
        playerVisual.SetActive(true);
        weaponVisual.SetActive(true);
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
    private void InstantiateProjectile(GameObject projectilePrefab, Transform pivot, float projectileVelocity, Vector3 direction)
    {
        GameObject instance = Instantiate(projectilePrefab, pivot.position, Quaternion.identity);
        instance.transform.LookAt(targetPosition, Vector3.up);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
    }

    // Instantiate projectiles to travel to specific point
    private void InstantiateProjectileTarget(GameObject projectilePrefab, Transform pivot, float projectileVelocity, Vector3 direction)
    {
        GameObject instance = Instantiate(projectilePrefab, new Vector3(pivot.position.x, pivot.position.y + 0.5f, pivot.position.z), Quaternion.identity);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
    }

    // Set projectile direction
    private void SetDirection()
    {
        // Get the direction for the projectile to fly to
        targetRotation = Mathf.Atan2(targetPosition.y - firePivot.position.y,
                                     targetPosition.x - firePivot.position.x) * Mathf.Rad2Deg;

        direction = (new Vector3(targetPosition.x, firePivot.position.y, targetPosition.z) - firePivot.position).normalized;
    }

    // Set projectile specific targeted direction
    private void SetDirectionTarget()
    {
        Vector3 hitPositionDirection = (targetPosition - firePivot.position).normalized;
        float distance = Vector3.Distance(targetPosition, transform.position);
        distance = Mathf.Min(distance, abilityRange);
        Vector3 newHitPosition = firePivot.position + hitPositionDirection * distance;

        direction = (newHitPosition - firePivot.position).normalized;
    }

    // Dalay basic attacks
    private IEnumerator DelayAttack()
    {
        animator.SetBool("Attack", true);
        SetDirection();

        yield return new WaitForSeconds(instanceDelay);

        InstantiateProjectile(projectile, firePivot, projectileVelocity, direction);
    }
    #endregion
}