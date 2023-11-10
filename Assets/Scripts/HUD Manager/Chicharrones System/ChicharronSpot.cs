using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChicharronSpot : MonoBehaviour
{
    public GameObject chicharronPrefab;
    public PlayerInfo player;
    ChicharronSelector chicharronImage;

    // Solo para testing, habilitar con controles (key: K)
    public bool healing = false;

    public void Start()
    {
        chicharronImage = chicharronPrefab.GetComponent<ChicharronSelector>();
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
        if (player.usosChicharron == 0)
        {
            return;
        }
        player.UseChicharron();
        Debug.Log("Chicharron: " + player.usosChicharron);
    }

    public void SetChicharron()
    {
        if (player.usosChicharron == 0)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.Empty);
        }
        else if (player.usosChicharron == 1)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.SemiEmpty);
        }
        else if (player.usosChicharron == 2)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.SemiFull);
        }
        else if (player.usosChicharron == 3)
        {
            chicharronImage.SetChicharronImage(ChicharronStatus.Full);
        }
        chicharronImage.transform.localScale = Vector3.one;
        // chicharronImage.transform.position = Vector3.zero;
    }
}
