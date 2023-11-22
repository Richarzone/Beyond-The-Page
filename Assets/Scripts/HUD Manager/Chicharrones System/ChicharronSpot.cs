using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChicharronSpot : MonoBehaviour
{
    public GameObject chicharronPrefab;
    public PlayerController player;
    ChicharronSelector chicharronImage;

    // Solo para testing, habilitar con controles (key: K)
    public bool healing = false;
    public void Start()
    {
        GameObject chicharronActual = Instantiate(chicharronPrefab);
        chicharronActual.transform.SetParent(transform);
        chicharronImage = chicharronActual.GetComponent<ChicharronSelector>();
        SetChicharron();
    }


    public void Update()
    {
        SetChicharron();
        if (healing == true) 
        {
            UseChicharron();
            healing = false;
        }
    }

    public void UseChicharron()
    {
        SetChicharron();
        if (player.usosChicharrones == 0 || player.health == player.maxHealth)
        {
            return;
        }
        player.UseChicharron();
        Debug.Log("Chicharron: " + player.usosChicharrones);
    }

    public void SetChicharron()
    {
        if (player.usosChicharrones == 0)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.Empty);
        }
        else if (player.usosChicharrones == 1)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.SemiEmpty);
        }
        else if (player.usosChicharrones == 2)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.SemiFull);
        }
        else if (player.usosChicharrones == 3)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.Full);
        }
        chicharronImage.transform.localScale = Vector3.one;
        // chicharronImage.transform.position = Vector3.zero;
    }
}
