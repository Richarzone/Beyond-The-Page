using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffClass : MonoBehaviour
{
    [SerializeField] private BuffSO buffSO;

    private void OnTriggerEnter(Collider collision)
    {
        if ((1 << collision.gameObject.layer) == buffSO.playerLayer.value)
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player.GetCurrentBuff() == null)
            {
                player.SetCurrentBuff(buffSO);
            }
            else
            {
                Instantiate(player.GetCurrentBuff().buffPrefab, transform.position, Quaternion.identity);
                player.SetCurrentBuff(buffSO);
            }
            
            Destroy(gameObject);
        }
    }
}