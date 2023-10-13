using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;
    public GameObject goober;
    void Start()
    {
        mainCamera = Camera.main;
        //transform.rotation = mainCamera.transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation;
    }

    private void Update()
    {
        
    }

}
