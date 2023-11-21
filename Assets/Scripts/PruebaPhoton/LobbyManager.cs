// Codigo que crea rooms
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    public GameObject connectToRoom;
    public GameObject lobbyMenu;

    [Header("UI Create Room")]
    public TextMeshProUGUI roomInputField;
    public Text roomName;

    [Header("UI Room Buttons")]
    public roomItem roomItemPrefab;
    List<roomItem> roomItemsList = new List<roomItem>();
    public Transform contentObject;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers =2 });
        }
    }

    // En caso de que si se cree el room y funcione la conexion se cambia de panel
    public override void OnJoinedRoom()
    {
        connectToRoom.SetActive(false);
        lobbyMenu.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }

    // Funcion que actualiza la room list
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (roomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            roomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
        
    }
}
