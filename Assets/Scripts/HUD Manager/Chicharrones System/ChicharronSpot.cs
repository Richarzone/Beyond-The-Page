using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static HeartSelector;

public class ChicharronSpot : MonoBehaviour
{
    public GameObject chicharronPrefab;
    public PlayerInfo player;
    ChicharronSelector chicharronImage = new ChicharronSelector();

    public bool healing = false;

    public void Awake()
    {
        DoChicharron();
    }

    public void Update()
    {
        if (healing == true) 
        {
            UseChicharron();
            healing = false;
        }
    }

    public void UseChicharron()
    {
        if (player.usosChicharron == 0)
        {
            return;
        }
        player.UseChicharron();
        DoChicharron();
        Debug.Log("Chicharron: " + player.usosChicharron);
    }

    public void DoChicharron()
    {
        ClearChicharron();
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


    public void ClearChicharron()
    {
        Destroy(gameObject);
        chicharronImage = new ChicharronSelector();
    }
}
