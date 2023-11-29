using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff")]
public class BuffSO : ScriptableObject
{
    public enum BuffType
    {
        Damage,
        Cooldown,
        Speed
    }

    public BuffType buffType;
    public GameObject buffPrefab;
    public LayerMask playerLayer;
    public AudioClip soundEffect;
    public GameObject buffVFX;
    public Sprite buffSprite;
    public float effectPercentage;
    public float effectDuration;
}
