using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviourPunCallbacks
{
    public GameObject mainWindow;
    public GameObject lobbyWindow;

    [Header("Main Menu")]
    public Button createRoomBtn;
    public Button joinRoomBtn;
    public Button backBtn;

    [Header("Lobby")]
    public Button startGameBtn;
    
    public TextMeshProUGUI playerTextList;

    [Header("ManagerObject")]
    public GameObject managerObject;

    public override void OnConnectedToMaster()
    {
        createRoomBtn.interactable = true;
        joinRoomBtn.interactable = true;
    }

    public void JoinRoom(TMP_InputField _roomName)
    {
        NetworkManager.instance.JoinRoom(_roomName.text);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }

    public void CreateRoom(TMP_InputField _roomName)
    {
        lobbyWindow.SetActive(true);
        mainWindow.SetActive(false);
        
        NetworkManager.instance.CreateRoom(_roomName.text);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);
    }
    
    public override void OnJoinedRoom()
    {
        lobbyWindow.SetActive(true);
        mainWindow.SetActive(false);
        photonView.RPC("UpdatePlayerInfo", RpcTarget.All);   
    }

    public void GetPlayerName(TMP_InputField _playerName)
    {
        PhotonNetwork.NickName = _playerName.text;
    }

    [PunRPC]
    public void UpdatePlayerInfo() 
    {
        playerTextList.text = "";
        foreach(Player player in PhotonNetwork.PlayerList) 
        {
            playerTextList.text += player.NickName + "\n";
        }
        if(PhotonNetwork.IsMasterClient) 
        {
            startGameBtn.interactable = true;
        }
        else
        {
            startGameBtn.interactable = false;
        }
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        
        lobbyWindow.SetActive (false);
        mainWindow.SetActive (true);
    }

    public void PressBack()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.Destroy(managerObject);
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame()
    {
        NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "Game View");
    }
}

