using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardMusketeer : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;
    public bool boolean;
    public GameObject musketeer;

    public float lerpSpeed;

    private Quaternion startRotation;
    public Quaternion endRotation;
    public float t = 0.0f;

    void Start()
    {
        mainCamera = Camera.main;
        boolean = true;
        transform.rotation = mainCamera.transform.rotation;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        
        if (boolean)
        {
            t += lerpSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(endRotation, startRotation, t);
            transform.rotation = mainCamera.transform.rotation;
            //startRotation = transform.rotation;
        }
        else if(transform.rotation != endRotation)
        {
            //if(t >= lerpSpeed)
            //{
            //    t = 0;
            //}
            t += lerpSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
        }

    }
}
