using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingThunderProjectile : Projectile
{
    private float maxDamage;

    void Start()
    {
        StartCoroutine(LifeTime());
        maxDamage = calculatedDamge * 8f;
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == enemyLayer.value)
        {
            other.GetComponent<EnemyClass>().Damage(calculatedDamge, baseDamage);
            if (calculatedDamge < maxDamage)
            {
                calculatedDamge += calculatedDamge;
            }
        }
    }
}
