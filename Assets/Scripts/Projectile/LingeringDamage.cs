using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LingeringDamage : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float timeBetweenDamage;
    [SerializeField] private float damage;
    [SerializeField] private float duration;
    [SerializeField] private LayerMask enemyMask;

    private float damageTime;
    private bool durationOver;

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private void Update()
    {
        if (damageTime >= 0f)
        {
            damageTime -= Time.deltaTime;
            return;
        }

        if (!durationOver)
        {
            Damage();
            damageTime = 1 / timeBetweenDamage;
        }

    }


    private void Damage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, enemyMask.value);

        foreach (Collider hitCollider in hitColliders)
        {
            hitCollider.GetComponent<EnemyClass>().Damage(damage, damage);
        }
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(duration);
        durationOver = true;
    }

    void OnDrawGizmos()
    {
        Color white = Color.white;
        white.a = 0.5f;
        Gizmos.color = white;
        Gizmos.DrawSphere(transform.position, range);
    }
}
