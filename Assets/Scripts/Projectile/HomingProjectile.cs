using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool hasImpactVFX;
    [SerializeField] private ParticleSystem impactVFX;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private bool ignoreEnemyLayer;

    private Rigidbody rb;
    private Transform target;
    private Transform origin;
    private Vector3 direction;
    private Quaternion rotation;
    private float baseDamage;
    private float calculatedDamge;
    private float projectileVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(LifeTime());
    }

    private void Update()
    {
        if (target != null)
        {
            direction = (new Vector3(target.position.x, origin.position.y, target.position.z) - origin.position).normalized;
            rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            rb.velocity = transform.forward * projectileVelocity;
        }
        
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == enemyLayer.value && target == collision.transform && !ignoreEnemyLayer)
        {
            collision.gameObject.GetComponent<EnemyClass>().Damage(calculatedDamge, baseDamage);

            if (hasImpactVFX)
            {
                ParticleSystem impact = Instantiate(impactVFX, new Vector3(transform.position.x, 0.2f, transform.position.z), impactVFX.transform.rotation);
                Destroy(impact.gameObject, impact.main.duration + impact.main.duration);
            }

            Destroy(gameObject);
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == enemyLayer.value)
        {
            other.GetComponent<EnemyClass>().Damage(calculatedDamge, baseDamage);
        }
    }

    public void SetDamage(float applyDamage, float damage)
    {
        baseDamage = damage;
        calculatedDamge = applyDamage;
    }

    public void SetVelocity(float velocity)
    {
        projectileVelocity = velocity;
    }

    public void SetOrigin(Transform transform)
    {
        origin = transform;
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
    }
}
