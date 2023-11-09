using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HeartSelector;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerInfo playerHealth;

    List<HeartSelector> hearts = new List<HeartSelector>();

    public void Awake()
    {
        DrawHeartsType(2);
    }

    // REMOVE WHEN WE FIND WHEN THE PLAYER IS ATTACKED< IT SHOULD BE CALLED THERE
    public void Update()
    {
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            int currentHealthInHearts = (int)Mathf.Clamp(playerHealth.health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)currentHealthInHearts);
        }
    }

    public void CreateHeartType(int num)
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);
        HeartSelector heartComponent = newHeart.GetComponent<HeartSelector>();

        if (num == 0)
            heartComponent.SetHeartImage(HeartStatus.Empty);
        else if (num == 1)
            heartComponent.SetHeartImage(HeartStatus.Half);
        else if (num == 2)
            heartComponent.SetHeartImage(HeartStatus.Full);
        heartComponent.transform.localScale = Vector3.one;
        hearts.Add(heartComponent);
    }

    public void DrawHeartsType(int num)
    {
        ClearHearts();

        float maxHealthRemainder = playerHealth.maxHealth % 2;
        int heartsToMake = (int)((playerHealth.maxHealth / 2) + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateHeartType(num);
        }

    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HeartSelector>();
    }

}
