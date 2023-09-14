using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingPiece : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image background;
    private int id;

    public Image GetIcon()
    {
        return icon;
    }

    public Image GetBackground()
    {
        return background;
    }

    public void SetID(int newID)
    {
        id = newID;
    }

    public int GetID()
    {
        return id;
    }
}