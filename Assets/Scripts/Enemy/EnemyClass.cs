using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class EnemyClass : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool infiniteHealth;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private float deathTime;
    private float damageMultiplyer;

    [Header("Damage VFX")]
    [SerializeField] private Transform damageAnimPivot;
    [SerializeField] private GameObject damageNormalAnim;
    [SerializeField] private GameObject damageCritAnim;
    [SerializeField] private GameObject damageSuperAnim;
    [SerializeField] private float minimunX;
    [SerializeField] private float maximumX;

    [Header("Hex UI")]
    [SerializeField] private List<Material> hexLevels = new List<Material>();
    [SerializeField] private GameObject hexCanvas;
    [SerializeField] private Image hexImage;
    [SerializeField] private Image hexBackground;
    private Animator hexAnimator;
    private Coroutine hexCorrutine;

    [Header ("Hitboxes")]
    [SerializeField] private float attackDamage;
    [SerializeField] private float hitboxRange;


    private float currentHealth;
    public float CurrentHealth 
    { 
        get
        {
            return currentHealth;
        }
    }
    public bool isGrounded;
    private int hexLevel;
    
    void Start()
    {
        currentHealth = health;
        damageMultiplyer = 1f;

        hexCanvas.SetActive(true);
        hexAnimator = hexCanvas.GetComponent<Animator>();
    }

    public void Damage(float applyDamage, float damageValue)
    {
        float trueDamage = applyDamage * damageMultiplyer;

        if (!infiniteHealth)
        {
            currentHealth -= trueDamage;
        }

        // Spawn damage effect
        float randomX = Random.Range(minimunX, maximumX);

        GameObject pivotInsatnce = new GameObject("Pivot Instance");
        pivotInsatnce.transform.position = new Vector3(damageAnimPivot.position.x + randomX, damageAnimPivot.position.y, damageAnimPivot.position.z);
        pivotInsatnce.transform.SetParent(damageAnimPivot);

        GameObject damageInstance = DamageAnimationInstance(trueDamage, damageValue);
        damageInstance.transform.SetParent(pivotInsatnce.transform);
        damageInstance.transform.localScale = Vector3.one;
        damageInstance.GetComponent<TextMeshProUGUI>().text = trueDamage.ToString();

        Destroy(pivotInsatnce, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);

        
        if (currentHealth <= 0)
        {
            damageAnimPivot.SetParent(null);
            enemyAnimator.GetComponent<NavMeshAgent>().isStopped = true;
            enemyAnimator.GetComponent<CapsuleCollider>().enabled = false;
            enemyAnimator.GetComponent<Animator>().SetTrigger("Death");
            Debug.Log(enemyAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            Destroy(damageAnimPivot.gameObject, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
            Destroy(gameObject, deathTime);
            
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

    public void ApplyHex(float hex, float increment, float duration)
    {
        if (hexLevel < 3)
        {
            damageMultiplyer = 1f + (hex + (hex * increment) * hexLevel);
            hexImage.material = hexLevels[hexLevel];
            hexLevel++;

            hexAnimator.Play("Hex Enter");

            if (hexCorrutine != null)
            {
                StopCoroutine(hexCorrutine);
            }

            hexCorrutine = StartCoroutine(Hex(duration));
        }
    }

    private IEnumerator Hex(float duration)
    {
        yield return new WaitForSeconds(duration);

        hexAnimator.Play("Hex Exit");
        damageMultiplyer = 1f;
        hexLevel = 0;
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == groundLayer)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == groundLayer)
        {
            isGrounded = false;
        }
    }

    public void EnemyMeleeAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, hitboxRange);

        foreach (Collider hitCollider in hitColliders)
        {

            if ((1 << hitCollider.gameObject.layer) == playerLayer.value)
            {
                Debug.Log(hitCollider.gameObject.layer);
                hitCollider.GetComponent<PlayerController>().DamagePlayer(attackDamage);
            }
        }
    }
}
