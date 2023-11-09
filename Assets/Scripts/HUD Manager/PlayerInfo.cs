using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static event Action OnPlayerDamage;
    public static event Action OnPlayerDeath;

    public float health, maxHealth = 10f;
    public int usosChicharron = 3;

    private void Start()
    {
        health = maxHealth;
        usosChicharron = 3;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        OnPlayerDamage?.Invoke();
    }

    public void UseChicharron()
    {
        if (health <= 7.5)
        {
            health += 2.5f;
        } 
        else
        {
            health = maxHealth;
        }
        usosChicharron--;
    }
}
