using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float lifeTime;
    [SerializeField] protected bool hasImpactVFX;
    [SerializeField] protected ParticleSystem impactVFX;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected bool ignoreEnemyLayer;
    [SerializeField] protected float attackDamage;

    protected float baseDamage;
    protected float calculatedDamge;
   

    void Start()
    {
        StartCoroutine(LifeTime());
    }

    public void SetDamage(float applyDamage, float damage)
    {
        baseDamage = damage;
        calculatedDamge = applyDamage;
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == enemyLayer.value && !ignoreEnemyLayer)
        {
            collision.gameObject.GetComponent<EnemyClass>().Damage(calculatedDamge, baseDamage);
        }
        else if ((1 << collision.gameObject.layer) == playerLayer.value)
        {
            collision.gameObject.GetComponent<PlayerController>().DamagePlayer(attackDamage);
        }

        /*Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashRange);

        foreach (Collider collider in hitColliders)
        {
            if (collider.tag == "Enemy")
            {
                Destroy(collider.gameObject);
            }
        }

        float pitch = Random.Range(0.85f, 1.15f);
        float volume = Random.Range(0.6f, 0.7f);

        GameObject impactPoint = new GameObject("Impact Sound");

        AudioSource impactAudioSource = impactPoint.AddComponent<AudioSource>();
        impactAudioSource.pitch = pitch;
        impactAudioSource.volume = volume;
        impactAudioSource.PlayOneShot(impactSound);

        Destroy(impactPoint, impactSound.length);*/

        if (hasImpactVFX)
        {
            ParticleSystem impact = Instantiate(impactVFX, new Vector3(transform.position.x, 0.2f, transform.position.z), impactVFX.transform.rotation);
            Destroy(impact.gameObject, impact.main.duration + impact.main.duration);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == enemyLayer.value)
        {
            other.GetComponent<EnemyClass>().Damage(calculatedDamge, baseDamage);
        }
       
    }
}
