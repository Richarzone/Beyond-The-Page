using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private bool hasImpactVFX;
    [SerializeField] private ParticleSystem impactVFX;

    void Start()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
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
            ParticleSystem impact = Instantiate(impactVFX, transform.position, impactVFX.transform.rotation);
            Destroy(impact.gameObject, impact.main.duration + impact.main.duration);
        }
        
        Destroy(gameObject);
    }
}
