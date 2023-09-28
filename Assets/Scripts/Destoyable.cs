using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoyable : MonoBehaviour
{
    public void DestoyMe()
    {
        Destroy(gameObject);
    }
}
