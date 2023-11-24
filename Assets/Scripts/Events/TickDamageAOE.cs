using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickDamageAOE : MonoBehaviour
{
    [SerializeField] private LayerMask effectLayer;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    private float applyDamage;

    private void AOETickDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider hitCollider in hitColliders)
        {
            if ((1 << hitCollider.gameObject.layer) == effectLayer.value)
            {
                hitCollider.GetComponent<EnemyClass>().Damage(applyDamage, damage);
            }
        }
    }

    public void SetApplyDamage(float value)
    {
        applyDamage = value;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public void SetRange(float value)
    {
        range = value;
    }
}
