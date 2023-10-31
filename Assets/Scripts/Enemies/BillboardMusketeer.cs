using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardMusketeer : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;

    public float lerpSpeed;
    private float currentSlerpProgress = 0.0f;

    public Quaternion targetRotation;

    public int lerpInt = 0;

    public Quaternion lookRotation;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            if (lerpInt == 0)
            {
                // Calculate the rotation to face the camera
                lookRotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);

                // Apply the rotation to the object
                transform.rotation = lookRotation;
            }
            else if(lerpInt == 1)
            {
                transform.rotation = Quaternion.Slerp(lookRotation, targetRotation, currentSlerpProgress);

                // Update the slerp progress based on the speed
                currentSlerpProgress += lerpSpeed * Time.deltaTime;
                currentSlerpProgress = Mathf.Clamp01(currentSlerpProgress);
            }
            else if(lerpInt == 2)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, currentSlerpProgress);

                // Update the slerp progress based on the speed
                currentSlerpProgress += lerpSpeed * Time.deltaTime;
                currentSlerpProgress = Mathf.Clamp01(currentSlerpProgress);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }

    private void LateUpdate()
    {
    }
}
