using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    //Instancia (singleton)
    public static GameController Instance;

    public bool isGameEnd = false;
    public string playerPrefab;
    public GameObject playerObj;

    public GameObject[] players;

    [SerializeField]private int playerInGame;

    public UIController uiController;//metodo de acceso sin convertir en singleton

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        players = new GameObject[PhotonNetwork.PlayerList.Length];
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    InGame();
        //}
        photonView.RPC("InGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void InGame()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length) 
        {
            SpawnPlayers();
        }
    }

    void SpawnPlayers() 
    {
        Debug.Log("Spawn player");
        playerObj = PhotonNetwork.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //players[PhotonNetwork.LocalPlayer.ActorNumber - 1] = playerObj.transform.GetChild(1).gameObject;

        //forma larga
        //playerObj.GetComponent<PlayerController>().photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    void GoBackToMenu()
    {
        Destroy(NetworkManager.instance.gameObject);
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.LoadScene("MenuScene");
    }

}
