using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilZone : MonoBehaviour
{
    private float radius;
    public float Radius
    {
        get { return radius; }
        set { radius = value; }
    }
    [SerializeField] private LayerMask detectionLayer; // Set the layers you want to detect in the Inspector.
    private Transform player;
    public Transform Player
    {
        get { return player; }
        set { player = value; }
    }

    private Collider[] colliders;
    public Collider[] Colliders
    {
        get { return colliders; }
    }
    void Update()
    {
        // Perform the overlap sphere check.
        colliders = Physics.OverlapSphere(transform.position, radius, detectionLayer);

        // Process the detected objects.
        foreach (Collider collider in colliders)
        {
            // Do something with the detected object.
            //Debug.Log("Detected: " + collider.gameObject.name);
            player = collider.gameObject.transform;
        }
    }

    // Visualize the detection area in the Scene view.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
