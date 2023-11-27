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
    public GameObject characterWindow;

    [Header("Main Menu")]
    public Button createRoomBtn;
    public Button joinRoomBtn;
    public Button backBtn;
    //public InputField roomInputField;

    [Header("Lobby")]
    public Button startGameBtn;

    //public TextMeshProUGUI playerTextList;

    [Header("ManagerObject")]
    public GameObject managerObject;

    [Header("Select Character")]
    public List<PlayerItem> PlayerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public override void OnConnectedToMaster()
    {
        createRoomBtn.interactable = true;
        joinRoomBtn.interactable = true;
    }

    public void JoinRoom(TMP_InputField _roomName)
    {
        NetworkManager.instance.JoinRoom(_roomName.text);
        photonView.RPC("UpdatePlayerList", RpcTarget.All);
        UpdatePlayerList();
    }

    public void CreateRoom(TMP_InputField _roomName)
    {
        lobbyWindow.SetActive(true);
        mainWindow.SetActive(false);

        NetworkManager.instance.CreateRoom(_roomName.text);
        //NetworkManager.instance.CreateRoom(_roomName.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
        photonView.RPC("UpdatePlayerList", RpcTarget.All);
        UpdatePlayerList();
    }
    
    /*public void CreateRoom()
    {
        if (roomInputField.text.Length >= 1)
        {
            lobbyWindow.SetActive(true);
            mainWindow.SetActive(false);

            //NetworkManager.instance.CreateRoom(_roomName.text);
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
            photonView.RPC("UpdatePlayerList", RpcTarget.All);
            UpdatePlayerList();
        }
    }*/

    public override void OnJoinedRoom()
    {
        lobbyWindow.SetActive(true);
        mainWindow.SetActive(false);
        photonView.RPC("UpdatePlayerList", RpcTarget.All);
        UpdatePlayerList();


    }

    public void GetPlayerName(TMP_InputField _playerName)
    {
        PhotonNetwork.NickName = _playerName.text;
    }
    /*public void UpdatePlayerInfo()
    {
        playerTextList.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerTextList.text += player.NickName + "\n";
        }
        if (PhotonNetwork.IsMasterClient)
        {
            startGameBtn.interactable = true;
        }
        else
        {
            startGameBtn.interactable = false;
        }
    }*/

    [PunRPC]
    void UpdatePlayerList()
    {
        foreach (PlayerItem item in PlayerItemsList)
        {
            Destroy(item.gameObject);
        }

        PlayerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            PlayerItemsList.Add(newPlayerItem);
        }

        // Start Button
        if (PhotonNetwork.IsMasterClient)
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
        lobbyWindow.SetActive(false);
        mainWindow.SetActive(true);
        UpdatePlayerList();
    }

    public void PressBack()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.Destroy(managerObject);
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame()
    {
        NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "EnemyTest");
    }



}