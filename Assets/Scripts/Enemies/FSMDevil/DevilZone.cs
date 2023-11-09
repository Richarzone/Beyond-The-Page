using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilZone : MonoBehaviour
{
    public float radius;
    public LayerMask detectionLayer; // Set the layers you want to detect in the Inspector.
    public Transform player;
    public Collider[] colliders;
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
