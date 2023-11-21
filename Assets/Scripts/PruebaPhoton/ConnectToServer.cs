// Codigo que nos conecta al servidor
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    public GameObject connectToServer;
    public GameObject connectToRoom;
    public GameObject lobbyMenu;

    [Header("UI PlayerName")]
    public TextMeshProUGUI usernameInput;
    public TextMeshProUGUI buttonText;

    // Funcion que nos conecta una vez que damos click
    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;

            // Se cambia el texto del boton
            buttonText.text = "Connecting...";
            // Se realiza conexion a photon
            PhotonNetwork.ConnectUsingSettings();

        }

    }

    // Funcion que nos cambia de escena si se logra la conexion a Photon
    public override void OnConnectedToMaster()
    {
        connectToRoom.SetActive(true);
        connectToServer.SetActive(false);
    }
}
