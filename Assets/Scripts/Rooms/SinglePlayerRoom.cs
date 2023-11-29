using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SinglePlayerRoom : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
    }

    

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.OfflineMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED TO MASTER");
        CreateRoom("SINGLEPLAYER");
    }

    public void CreateRoom(string _name)
    {
        PhotonNetwork.CreateRoom(_name);
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        Debug.Log(string.Format("Created Room: {0}", PhotonNetwork.CurrentRoom.Name));
    }
}
