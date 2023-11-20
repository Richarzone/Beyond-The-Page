using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityClass : MonoBehaviour
{
    [SerializeField] protected float abilityCooldown;

    //Cooldown UI variables
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Sprite skillSprite;

    // Skill input
    protected bool skillInput;
    // Switch to detect if skill is active or not
    protected bool activeSkill;
    // Switch to control if the player can use the skill
    protected bool blockSkill;
    // ID of the button that called this ability
    protected int skillButton;
    // Identifier if current ability is on cooldown
    protected bool isCooldown;
    // Ui button cooldown timer
    private float cooldownTimer;

    protected CharacterClass characterClass;

    protected void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    private void Awake()
    {
        characterClass = GetComponent<CharacterClass>();
    }

    protected void Update()
    {
        if(isCooldown)
        {
            ApplyCooldown();
        }
    }

    public virtual void UseAbility()
    {

    }

    protected void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
        imageCooldown.fillAmount = cooldownTimer / abilityCooldown;
    }

    protected void ActivateCooldown()
    {
        isCooldown = true;
        textCooldown.gameObject.SetActive(true);
        cooldownTimer = abilityCooldown;
    }

    protected void ResetCooldown()
    {
        isCooldown = false;
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    protected void CallCooldown()
    {
        characterClass.GetAbilityManager().AbilityCoroutineManager(CooldownCoroutine());
    }

    public virtual IEnumerator TwinSpellCoroutine(CharacterClass character, TwinSpell ability)
    {
        yield return null;
    }

    public IEnumerator CooldownCoroutine()
    {
        cooldownTimer = abilityCooldown;
        textCooldown.gameObject.SetActive(true);

        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / abilityCooldown;
            yield return null;
        }
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    public void SkillInput(bool input)
    {
        skillInput = input;
    }

    public bool GetSkillInput()
    {
        return skillInput;
    }

    public void ActiveSkill(bool active)
    {
        activeSkill = active;
    }

    public void AbilityButtonID(int id)
    {
        skillButton = id;
    }

    public void ChangeIcon()
    {
        skillIcon.sprite = skillSprite;
    }
}