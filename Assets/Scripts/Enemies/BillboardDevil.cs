using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardDevil : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        transform.rotation = new Quaternion(0f, mainCamera.transform.rotation.y, mainCamera.transform.rotation.z,mainCamera.transform.rotation.w) ;
    }
}
