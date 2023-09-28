using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageClass : CharacterClass
{
    [SerializeField] private Transform firePivot;
    [SerializeField] private Transform vfxPivot;
    [SerializeField] private LayerMask groundLayer;

    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileVelocity;

    [Header("Mass Hex")]
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private float hexRange;
    [SerializeField] private float hexAOE;

    [Header("Rolling Thunder")]
    [SerializeField] private GameObject thunderPrefab;
    [SerializeField] private float thunderDamage;
    [SerializeField] private float thunderVelocity;
    [SerializeField] private float thunderRange;

    [Header("Twin Spell")]
    [SerializeField] private ParticleSystem twinSpellVFX;

    [Header("UI")]
    [SerializeField] private Canvas abilityCanvas;
    [SerializeField] private Image abilityRangeIndicator;
    [SerializeField] private float abilityRange;

    [Header("VFX")]
    [SerializeField] private ParticleSystem smokeBombVFX;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject weaponVisual;

    private Vector3 targetPosition;
    private Vector3 habilityTargetPosition;
    private Vector3 direction;
    private float targetRotation;

    private void Start()
    {
        
    }

    private void Update()
    {
        // Attacks
        if (attack)
        {
            Attack();
        }
        else
        {
            animator.SetBool("Attack", false);
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
        animator.SetBool("Attack", true);
        
        if (timeBetweenAttacks <= 0f)
        {
            SetDirection();
            InstantiateProjectile(projectile, firePivot, projectileVelocity, direction, attackDamage);
            
            timeBetweenAttacks = 1 / attackSpeed;
        }
    }

    #region Skills
    private void AimSkill1()
    {
        abilityRangeIndicator.gameObject.transform.localScale = new Vector3(hexRange, hexRange, hexRange);
        abilityCanvas.transform.localScale = new Vector3(hexAOE, hexAOE, hexAOE);
        abilityCanvas.enabled = true;
        abilityRangeIndicator.enabled = true;

        Vector3 hitPositionDirection = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(targetPosition, transform.position);
        distance = Mathf.Min(distance, abilityRange);
        habilityTargetPosition = transform.position + hitPositionDirection * distance;

        abilityCanvas.transform.position = new Vector3(habilityTargetPosition.x, 0.01f, habilityTargetPosition.z);

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
        //abilityRangeIndicator.gameObject.transform.localScale = new Vector3(stunRange, stunRange, stunRange);
        //abilityCanvas.enabled = true;
        //abilityRangeIndicator.enabled = true;

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
        InstantiateOnPoint(hexPrefab, habilityTargetPosition);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill1 = false;
    }

    protected override IEnumerator Skill2()
    {
        lockSkill2 = true;
        SetDirection();
        InstantiateProjectile(thunderPrefab, firePivot, thunderVelocity, direction, thunderDamage);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill2 = false;
    }

    protected override IEnumerator Skill3()
    {
        lockSkill3 = true;

        ParticleSystem vfxTwinSpellInstance = Instantiate(twinSpellVFX, vfxPivot.position, twinSpellVFX.transform.rotation);
        vfxTwinSpellInstance.transform.parent = vfxPivot;
        Destroy(vfxTwinSpellInstance.gameObject, vfxTwinSpellInstance.main.duration + vfxTwinSpellInstance.main.startLifetime.constant);

        yield return new WaitForSeconds(habilityCooldown);

        lockSkill3 = false;
    }
    #endregion

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
        instance.GetComponent<Projectile>().SetDamage(damage, damage);
    }

    // Instantiate object on specific point
    private void InstantiateOnPoint(GameObject projectilePrefab, Vector3 point)
    {
        Instantiate(projectilePrefab, new Vector3(point.x, point.y + 0.5f, point.z), projectilePrefab.transform.rotation);
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
    #endregion
}