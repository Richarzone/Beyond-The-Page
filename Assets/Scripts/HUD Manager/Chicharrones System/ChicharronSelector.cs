using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChicharronSelector : MonoBehaviour
{
    [Header("Chicharrones")]
    public Sprite fullChicharron;
    public Sprite semiFullChicharron;
    public Sprite semiEmptyChicharron;
    public Sprite emptyChicharron;
    Image chicharronImage;

    private void Awake()
    {
        chicharronImage = GetComponent<Image>();
    }

    public void SetChicharronImage(ChicharronStatus status)
    {
        switch (status)
        {
            case ChicharronStatus.Empty:
                chicharronImage.sprite = emptyChicharron;
                break;
            case ChicharronStatus.SemiEmpty:
                chicharronImage.sprite = semiEmptyChicharron;
                break;
            case ChicharronStatus.SemiFull:
                chicharronImage.sprite = semiFullChicharron;
                break;
            case ChicharronStatus.Full:
                chicharronImage.sprite = fullChicharron;
                break;
        }
    }
}

public enum ChicharronStatus
{
    Empty = 0,
    SemiEmpty = 1,
    SemiFull = 2,
    Full = 3
}