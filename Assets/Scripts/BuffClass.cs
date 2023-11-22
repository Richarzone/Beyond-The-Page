using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffClass : MonoBehaviour
{
    public enum BuffType
    {
        Damage,
        Cooldown,
        Speed
    }

    [SerializeField] private BuffType buffType;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private float effectPercentage;
    [SerializeField] private float effectDuration;

    public BuffType GetBuffType()
    {
        return buffType;
    }

    public AudioClip GetSoundEffect()
    {
        return soundEffect;
    }

    public float GetEffectPercentage()
    {
        return effectPercentage;
    }

    public float GetEffectDuration()
    {
        return effectDuration;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((1 << collision.gameObject.layer) == playerLayer.value)
        {
            collision.GetComponent<PlayerController>().SetCurrentBuff(this);
        }
    }
}