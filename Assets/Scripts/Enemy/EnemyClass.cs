using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyClass : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool infiniteHealth;

    [Header("Damage VFX")]
    [SerializeField] private Transform damageAnimPivot;
    [SerializeField] private GameObject damageNormalAnim;
    [SerializeField] private GameObject damageCritAnim;
    [SerializeField] private GameObject damageSuperAnim;

    [SerializeField] private float minimunX;
    [SerializeField] private float maximumX;

    private float currentHealth;
    
    void Start()
    {
        currentHealth = health;
    }

    public void Damage(float applyDamage, float damageValue)
    {
        if (!infiniteHealth)
        {
            currentHealth -= applyDamage;
        }

        float randomX = Random.Range(minimunX, maximumX);

        GameObject pivotInsatnce = new GameObject("Pivot Instance");
        pivotInsatnce.transform.position = new Vector3(damageAnimPivot.position.x + randomX, damageAnimPivot.position.y, damageAnimPivot.position.z);
        pivotInsatnce.transform.SetParent(damageAnimPivot);


        GameObject damageInstance = DamageAnimationInstance(applyDamage, damageValue);
        damageInstance.transform.SetParent(pivotInsatnce.transform);
        damageInstance.transform.localScale = Vector3.one;
        damageInstance.GetComponent<TextMeshProUGUI>().text = applyDamage.ToString();

        Destroy(pivotInsatnce, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);

        if (currentHealth <= 0)
        {
            damageAnimPivot.SetParent(null);
            Destroy(damageAnimPivot.gameObject, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
            Destroy(gameObject);
        }
    }

    private GameObject DamageAnimationInstance(float applyDamage, float damageValue)
    {
        if ((applyDamage / damageValue) >= 3f)
        {
            return Instantiate(damageSuperAnim, transform.position, Quaternion.identity);
        }
        if ((applyDamage / damageValue) >= 2f)
        {
            return Instantiate(damageCritAnim, transform.position, Quaternion.identity);
        }
        else
        {
            return Instantiate(damageNormalAnim, transform.position, Quaternion.identity);
        }
    }
}
