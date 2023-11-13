using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static event Action OnPlayerDamage;
    public static event Action OnPlayerDeath;

    public int health, maxHealth = 10;
    public int usosChicharron = 3;

    private void Start()
    {
        health = maxHealth;
        usosChicharron = 3;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        OnPlayerDamage?.Invoke();
    }

    public void UseChicharron()
    {
        if (health <= 7.5)
        {
            health += 3;
        } 
        else
        {
            health = maxHealth;
        }
        usosChicharron--;
    }
}
