using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region SingletonPattern

    public static NetworkManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) 
        { 
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    #endregion


    #region ConnectionToServer

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
    /*
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("connection stablished");
        CreateRoom("testRoom");

    }
    */

    #endregion

    #region RoomLogic

    public void CreateRoom(string _name)
    {
        PhotonNetwork.CreateRoom(_name);        
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        Debug.Log(string.Format("Created Room: {0}", PhotonNetwork.CurrentRoom.Name));
    }

    #endregion

    #region ConnectionToRoom

    public void JoinRoom(string _name)
    {
        PhotonNetwork.JoinRoom(_name);
    }

    [PunRPC]
    public void LoadScene(string _nameScene) 
    {
        PhotonNetwork.LoadLevel(_nameScene);
    }

    #endregion


}
