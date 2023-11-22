using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSelector : MonoBehaviour
{
    [Header("Hearts")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("BuffHearts")]
    public Sprite buffHalfHeart;
    public Sprite buffFullHeart;

    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartStatus.ExtraHalf:
                heartImage.sprite = buffHalfHeart;
                break;
            case HeartStatus.ExtraFull:
                heartImage.sprite = buffFullHeart;
                break;
        }
    }
}

public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2,
    ExtraHalf = 3,
    ExtraFull = 4
}