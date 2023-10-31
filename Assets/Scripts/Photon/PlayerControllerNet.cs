using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerNet : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;

    [Header("Components")]
    public Rigidbody rig;
    public Player photonPlayer;


    // Inicializar la informacion del player actual
    [PunRPC]
    public void Init(Player player)
    {
        photonPlayer = player;// Asiganar el player actual
        id = player.ActorNumber;//Guardar el id del player
        GameController.instance.players[id - 1] = this;// Asiganarlo a las lista de player dentro del game controller

        if (!photonView.IsMine) // Verificar si el movimiento es del usuario actual
        {
            rig.isKinematic = true;
        }
    }


    /// Ocultar el objeto desde su Id de PhotonView    
    [PunRPC]
    void Hidewall(int _id)
    {
        GameObject obj = PhotonNetwork.GetPhotonView(_id).gameObject;//Obtener el Game object desde photon
        obj.gameObject.SetActive(false); //Desactivar el elemento
        GameController.instance.WinGame(id);
    }

}