using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClass : CharacterClass
{
    [SerializeField] private Transform firePivot;

    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileVelocity;

    private Vector3 targetPosition;
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
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f))
        {
            targetPosition = raycastHit.point;
        }

        //Debug.Log(timeBetweenAttacks);

        // If the time between attacks is zero or less than zero return
        if (timeBetweenAttacks <= 0f)
        {
            return;
        }

        // Refresh the time between attacks
        timeBetweenAttacks -= Time.deltaTime;
    }

    protected override void Attack()
    {
        animator.SetBool("Attack", true);
        
        if (timeBetweenAttacks <= 0f)
        {
            SetDirection();
            InstantiateProjectile(firePivot, targetRotation, direction);
            
            timeBetweenAttacks = 1 / attackSpeed;
        }
    }

    private void InstantiateProjectile(Transform pivot, float targetRotation, Vector3 direction)
    {
        GameObject instance = Instantiate(projectile, pivot.position, Quaternion.identity);
        instance.transform.LookAt(targetPosition, Vector3.up);
        instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
    }

    private void SetDirection()
    {
        // Get the direction for the projectile to fly to
        targetRotation = Mathf.Atan2(targetPosition.y - firePivot.position.y,
                                     targetPosition.x - firePivot.position.x) * Mathf.Rad2Deg;

        Debug.Log(targetPosition);

        direction = (new Vector3(targetPosition.x, firePivot.position.y, targetPosition.z) - firePivot.position).normalized;
    }
}