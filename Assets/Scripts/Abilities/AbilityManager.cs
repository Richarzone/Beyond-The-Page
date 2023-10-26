using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilityClass lastUsedSkill;

    public void AbilityCoroutineManager(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public AbilityClass LastUsedSkill
    {
        get { return lastUsedSkill; }
        set { lastUsedSkill = value; }
    }
}