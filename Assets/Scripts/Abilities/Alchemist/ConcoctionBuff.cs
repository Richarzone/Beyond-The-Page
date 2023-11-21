using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcoctionBuff : MonoBehaviour
{
    private float damageBuff;
    private float buffDuration;

    public void ApplyBuff()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.1f);
        hitColliders[0].GetComponent<PlayerController>().StartConcotionBuff(buffDuration, damageBuff);

        Destroy(gameObject);
    }

    public void SetDamageValue(float value)
    {
        damageBuff = value;
    }

    public void SetBuffDuration(float value)
    {
        buffDuration = value;
    }    
}