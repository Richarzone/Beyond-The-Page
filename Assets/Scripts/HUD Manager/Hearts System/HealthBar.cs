using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HeartSelector;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerController player;

    private int refreshIndicator = 10;

    public bool extraHeartsBoolean = false;

    public List<HeartSelector> hearts = new List<HeartSelector>();

    public void Awake()
    {
        DrawHearts(2);
    }

    public void Update()
    {
        if ((player.health + player.extraHealth) != refreshIndicator)
        {
            UpdateHearts();
            UpdateExtraHearts();

            refreshIndicator = player.health + player.extraHealth;
        }
        
        
        // Extra Hearts Auxiliary boolean extraHeartsBoolean
        if (extraHeartsBoolean)
        {
            DrawExtraHearts(2);
            extraHeartsBoolean = false;
        }
    }

    public void DrawExtraHearts(int num)
    {
        for (int i = 0; i < num; i++)
        {
            player.extraHealth += 2;
            CreateHeartType(4);
        }
        refreshIndicator = player.health + player.extraHealth;
    }

    public void DrawHearts(int num)
    {
        ClearHearts();
        float maxHealthRemainder = player.maxHealth % 2;
        int heartsToMake = (int)((player.maxHealth / 2) + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateHeartType(num);
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < 5; i++)
        {
            int currentHealthInHearts = (int)Mathf.Clamp(player.health - (i * 2), 0, 2);    
            hearts[i].SetHeartImage((HeartStatus)currentHealthInHearts);
            
        }
    }

    public void UpdateExtraHearts()
    {
        for (int i = 5; i < hearts.Count; i++)
        {
            int currentHealthInHearts = (int)Mathf.Clamp(player.extraHealth - ((i - 5) * 2), 0, 2);
            if (currentHealthInHearts == 0)
            {
                Destroy(hearts[i].gameObject);
                hearts.RemoveAt(i);
            }
            else
            {
                hearts[i].SetHeartImage((HeartStatus)currentHealthInHearts + 2);
            }
        }
    }


    private void CreateHeartType(int num)
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);
        HeartSelector heartComponent = newHeart.GetComponent<HeartSelector>();

        if (num == 0)
        {
            heartComponent.SetHeartImage(HeartStatus.Empty);
        }
        else if (num == 1)
        {
            heartComponent.SetHeartImage(HeartStatus.Half);
        }
        else if (num == 2)
        {
            heartComponent.SetHeartImage(HeartStatus.Full);
        }
        else if (num == 3)
        {
            heartComponent.SetHeartImage(HeartStatus.ExtraHalf);
        }
        else if (num == 4)
        {
            heartComponent.SetHeartImage(HeartStatus.ExtraFull);
        }
        heartComponent.transform.localScale = Vector3.one;
        hearts.Add(heartComponent);
    }

    private void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HeartSelector>();
    }

}
