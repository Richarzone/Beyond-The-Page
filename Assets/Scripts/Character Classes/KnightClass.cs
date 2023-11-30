using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightClass : CharacterClass
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check the input for basic attack attack
        if (attack && !blockAttack)
        {
            Attack();
        }
        
        // If the time between attacks is zero or less than zero return
        if (timeBetweenAttacks > 0f)
        {
            // Refresh the time between attacks
            timeBetweenAttacks -= Time.deltaTime;
        }
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

    protected override IEnumerator Dodge(float dodgeDuration)
    {
        blockClassChange = true;
        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(dodgeDuration);

        blockClassChange = false;
    }
}
