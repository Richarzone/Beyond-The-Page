using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatTrickProjectile : MonoBehaviour
{
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private float maxDistance;
    private bool arrived;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    void Update()
    {
        float distanceTraveled = Vector3.Distance(initialPosition, transform.position);

        if (distanceTraveled >= maxDistance && !arrived)
        {
            rb.velocity = Vector3.zero;
            animator.SetTrigger("Arrived");
            GetComponent<CapsuleCollider>().enabled = true;

            arrived = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((1 << collider.gameObject.layer) == playerLayer.value)
        {
            arrived = false;
            Destroy(gameObject);
        }
    }

    public void SetMaxDistance(float distance)
    {
        maxDistance = distance;
    }

    public bool Arrived()
    {
        return arrived;
    }
}