using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject endWindow;
    public PlayerData playerData;
    public Transform listPlayers;

    // Start is called before the first frame update
    void Start()
    {
        endWindow.SetActive(false);
    }

    public void InstancePlayers(string _playerName, bool _isWinner = false)
    {       
        PlayerData newPLayer = Instantiate(playerData, listPlayers) as PlayerData;

    }

    public void ShowEndView()
    {
        endWindow.SetActive(true);
    }
}
