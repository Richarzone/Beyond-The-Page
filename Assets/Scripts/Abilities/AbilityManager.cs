using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("Manager Configuration")]
    [SerializeField] private bool blockAbilitySlots;
    [SerializeField] protected bool globalCooldown;
    [SerializeField] protected float abilityCooldown;

    [Header("Last Used Skill")]
    [SerializeField] private AbilityClass lastUsedSkill;

    private PlayerController playerController;

    private void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    public void AbilityCoroutineManager(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public AbilityClass LastUsedSkill
    {
        get { return lastUsedSkill; }
        set { lastUsedSkill = value; }
    }

    public bool BlockAbilitySlots()
    {
        return blockAbilitySlots;
    }
}