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

    public Quaternion spawnPosition;
    public PlayerControllerNet[] players;

    [SerializeField]private int playerInGame;

    public UIController uiController;//metodo de acceso sin convertir en singleton

    void Awake()
    {
        spawnPosition = Quaternion.identity;
        Instance = this;
    }

    void Start()
    {
        players = new PlayerControllerNet[PhotonNetwork.PlayerList.Length];
        photonView.RPC("InGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void InGame()
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
        //GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab, spawnPositions[0].position, Quaternion.identity);
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); 
        
        //forma larga
        //playerObj.GetComponent<PlayerController>().photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer);

        //uso de variable para facil lectura
        PlayerControllerNet playScript = playerObj.GetComponent<PlayerControllerNet>();
        playScript.photonView.RPC("Init", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public void WinGame(int _playerId)
    {
        //Win
        //ActivateUI
        CreateTable(_playerId);
        //BackToMenu
        Invoke("GoBackToMenu", 3f);
    }

    void CreateTable(int _playerId)
    {
        uiController.ShowEndView();
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (_playerId == PhotonNetwork.PlayerList[i].ActorNumber)
            {
                uiController.InstancePlayers(PhotonNetwork.PlayerList[i].NickName, true);
            }
        }
        InsertAllPlayers(_playerId);
    }


    void InsertAllPlayers(int _playerId)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (_playerId != PhotonNetwork.PlayerList[i].ActorNumber)
            {
                uiController.InstancePlayers(PhotonNetwork.PlayerList[i].NickName);
            }
        }        
    }

    void GoBackToMenu()
    {
        Destroy(NetworkManager.instance.gameObject);
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.LoadScene("MenuScene");
    }

}
