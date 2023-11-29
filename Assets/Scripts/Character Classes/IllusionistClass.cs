using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionistClass : CharacterClass
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Attacks
        if (attack)
        {
            Attack();
        }

        // If the time between attacks is zero or less than zero return
        if (timeBetweenAttacks <= 0f)
        {
            return;
        }

        // Refresh the time between attacks
        timeBetweenAttacks -= Time.deltaTime;
    }

    protected override void Attack()
    {
        if (timeBetweenAttacks <= 0f)
        {
            audioSource.Play();
            animator.SetTrigger("Attack");
            timeBetweenAttacks = 1 / (attackSpeed * AttackSpeed());
        }
    }
}