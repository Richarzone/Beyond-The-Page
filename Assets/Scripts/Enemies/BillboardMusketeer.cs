using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardMusketeer : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;

    public Quaternion targetRotation;
    private Quaternion _targetRotation = Quaternion.identity;

    public int lerpInt = 0;

    public float turningRate = 30f;

    public Quaternion lookRotation;

    public Quaternion parentRotation;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            if (lerpInt == 0) //Billboard
            {
                // Calculate the rotation to face the camera
                lookRotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);

                // Apply the rotation to the object
                transform.rotation = lookRotation;
            }
            else if(lerpInt == 1) //Exit Billboard
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, turningRate * Time.deltaTime);
                if (transform.rotation.x <= .02f)
                {
                    lerpInt = 3;
                }
            }
            else if(lerpInt == 2) //Return to Billboard
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turningRate * Time.deltaTime);
                if (transform.rotation.x >= .45f)
                {
                    lerpInt = 0;
                }
            }
            else //No billboard
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }

    // Call this when you want to turn the object smoothly.
    public void SetBlendedEulerAngles(Vector3 angles)
    {
        _targetRotation = Quaternion.Euler(angles);
    }

    private void LateUpdate()
    {
    }
}
