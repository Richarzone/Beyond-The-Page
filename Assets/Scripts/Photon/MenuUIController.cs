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

    [Header("Lobby")]
    public Button startGameBtn;

    public TextMeshProUGUI playerTextList;

    [Header("ManagerObject")]
    public GameObject managerObject;

    [Header("Select Character")]
    [SerializeField] private Button playButtonCharacterSelect;

    [SerializeField] private GameObject felixButton;
    [SerializeField] private GameObject sophieButton;

    [SerializeField] private Sprite felixPruebaSprite;
    [SerializeField] private Sprite sophiePruebaSprite;

    [SerializeField] private bool felixSelected = false;
    [SerializeField] private bool sophieSelected = false;

    private Image felixImage;
    private Button felixInfoButton;
    private Image sophieImage;
    private Button sophieInfoButton;
    private Sprite felixSprite;
    private Sprite sophieSprite;

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
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();

        lobbyWindow.SetActive(false);
        mainWindow.SetActive(true);
    }

    public void PressBack()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.Destroy(managerObject);
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame()
    {
        lobbyWindow.SetActive(false);
        //characterWindow.SetActive(true);
        NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "Game View");
    }

    // Funciones Character Select
    // Se inician las imagenes predeterminadas de los botones de Felix y Sophie
    private void Start()
    {
        /*Cursor.visible = false;
        startButtonAnimator = startButton.GetComponent<Animator>();*/
        //felixButton.spriteState = sprStateFelix;
        felixImage = felixButton.GetComponent<Image>();
        felixInfoButton = felixButton.GetComponent<Button>();
        sophieImage = sophieButton.GetComponent<Image>();
        sophieInfoButton = sophieButton.GetComponent<Button>();

    }

    public void FelixButton()
    {
        felixSelected = true;
        sophieSelected = false;

        playButtonCharacterSelect.gameObject.SetActive(true);
        felixImage.sprite = felixInfoButton.spriteState.pressedSprite;
        sophieImage.sprite = sophiePruebaSprite;
    }

    public void SophieButton()
    {
        felixSelected = false;
        sophieSelected = true;

        playButtonCharacterSelect.gameObject.SetActive(true);
        sophieImage.sprite = sophieInfoButton.spriteState.pressedSprite;
        felixImage.sprite = felixPruebaSprite;
    }

    // Play button character select que envia al juego
    public void PressPlayCharacterSelect()
    {
        //StartCoroutine(GameStart());
        //sceneManager.LoadNextScene();
        if (felixSelected == false && sophieSelected == false)
        {
            playButtonCharacterSelect.gameObject.SetActive(false);
        }
        else
        {
            playButtonCharacterSelect.gameObject.SetActive(true);
            //NetworkManager.instance.photonView.RPC("LoadScene", RpcTarget.All, "Game View");
        }
    }


}