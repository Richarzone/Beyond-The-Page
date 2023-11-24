using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: ------ CREATE INTERFACE FOR PROJECTILES THAT DERIVE FROM THIS CLASS ------
// TO-CHECK: ------ CUSTOM INSPECTOR CLASS FOR ENEMY PROJECTILES USING THIS CLASS ------

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] protected float lifeTime;
    [SerializeField] protected bool hasImpactVFX;
    [SerializeField] protected ParticleSystem impactVFX;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected bool ignoreEnemyLayer;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected bool isLingering;
    [SerializeField] protected bool isExplosive;

    protected float baseDamage;
    protected float calculatedDamge;
    private CharacterClass instanceOwner;
    private float explosionAOE;

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    public void SetDamage(float applyDamage, float damage)
    {
        baseDamage = damage;
        calculatedDamge = applyDamage;
    }

    public void SetAOERange(float value)
    {
        explosionAOE = value;
    }

    public void SetInstanceOwner(CharacterClass value)
    {
        instanceOwner = value;
    }

    protected IEnumerator LifeTime()
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

            if (isLingering)
            {
                impact.gameObject.GetComponent<LingeringDamage>().SetInstanceOwner(instanceOwner);
            }

            Debug.Log(impact.main.duration);
            Destroy(impact.gameObject, impact.main.duration + impact.main.duration);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == enemyLayer.value && !ignoreEnemyLayer)
        {
            other.GetComponent<EnemyClass>().Damage(calculatedDamge, baseDamage);
        }
        else if ((1 << other.gameObject.layer) == groundLayer.value)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionAOE);

            foreach (Collider hitCollider in hitColliders)
            {
                Debug.Log(hitCollider.name);
                if ((1 << hitCollider.gameObject.layer) == enemyLayer.value)
                {
                    hitCollider.GetComponent<EnemyClass>().Damage(attackDamage +
                                                                 (attackDamage * instanceOwner.GetAbilityManager().GetPlayerController().DamageMultiplier()) +
                                                                 (attackDamage * instanceOwner.GetConcoctionDamageMultiplier()), attackDamage);
                }
            }

            if (hasImpactVFX)
            {
                ParticleSystem impact = Instantiate(impactVFX, new Vector3(transform.position.x, 0.2f, transform.position.z), impactVFX.transform.rotation);

                if (isLingering)
                {
                    impact.gameObject.GetComponent<LingeringDamage>().SetInstanceOwner(instanceOwner);
                }

                Destroy(impact.gameObject, impact.main.duration + impact.main.duration);
            }

            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Color white = Color.white;
        white.a = 0.5f;
        Gizmos.color = white;
        Gizmos.DrawSphere(transform.position, explosionAOE);
    }
}
