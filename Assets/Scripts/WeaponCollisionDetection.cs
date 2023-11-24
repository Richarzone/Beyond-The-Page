using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionDetection : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private LayerMask enemyLayer;
    private bool blockNormalDamage;

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == enemyLayer.value && !blockNormalDamage)
        {
            other.GetComponent<EnemyClass>().Damage(characterClass.CurrentAttackDamage(), characterClass.AttackDamage());
        }
    }

    public void BlockDamage()
    {
        blockNormalDamage = !blockNormalDamage;
    }
}