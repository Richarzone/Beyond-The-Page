using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionDetection : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private LayerMask enemyLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == enemyLayer.value)
        {
            other.GetComponent<EnemyClass>().Damage(characterClass.AttackDamage(), characterClass.AttackDamage());
        }
    }
}