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
    [SerializeField] private GameObject parent;
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

    [Header("Hitboxes")]
    [SerializeField] private Transform hitboxPosition;
    [SerializeField] private int attackDamage;
    [SerializeField] private float hitboxRange;
    [SerializeField] private bool hit;

    private float currentHealth;

    private bool canBeStuned;
    public bool CanBeStuned
    {
        get { return canBeStuned; }
        set { canBeStuned = value; }
    }

    public float CurrentHealth 
    { 
        get
        {
            return currentHealth;
        }
    }

    private bool canBeKnocked;
    public bool CanBeKnocked
    {
        get { return canBeKnocked; }
        set { canBeKnocked = value; }
    }

    private Vector3 force;
    public Vector3 Force
    {
        get { return force; }
        set { force = value; }
    }

    public bool isGrounded;
    private int hexLevel;

    private void Awake()
    {
        currentHealth = health;
        damageMultiplyer = 0f;

        hexCanvas.SetActive(true);
        hexAnimator = hexCanvas.GetComponent<Animator>();
    }

    public void Damage(float applyDamage, float damageValue)
    {
        float trueDamage = applyDamage + (damageValue * damageMultiplyer);

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
            //damageAnimPivot.SetParent(null);
            GameManager.Instance.EnemyAmount--;
            enemyAnimator.GetComponent<NavMeshAgent>().isStopped = true;
            enemyAnimator.GetComponent<CapsuleCollider>().enabled = false;
            enemyAnimator.GetComponent<Animator>().SetTrigger("Death");

            if (parent != null)
            {
                //Destroy(damageAnimPivot.gameObject, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
                Destroy(parent, deathTime);
            }
            else
            {
                //Destroy(damageAnimPivot.gameObject, damageInstance.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
                Destroy(gameObject, deathTime);
            }
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
            damageMultiplyer = hex + (hex * increment) * hexLevel;
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
        damageMultiplyer = 0f;
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

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == playerLayer.value && !hit)
        {
            Debug.Log(other.gameObject.layer);
            other.GetComponent<PlayerController>().DamagePlayer(attackDamage);
            hit = true;
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

    public void EnemyMeleeWeaponAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitboxPosition.transform.position, hitboxRange);

        foreach (Collider hitCollider in hitColliders)
        {

            if ((1 << hitCollider.gameObject.layer) == playerLayer.value)
            {
                Debug.Log(hitCollider.gameObject.layer);
                hitCollider.GetComponent<PlayerController>().DamagePlayer(attackDamage);
            }
        }
    }

    private void SetFalse()
    {
        hit = false;
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
