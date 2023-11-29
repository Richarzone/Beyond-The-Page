using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    private int attempts = 3;

    public int seed;
    public int Attempts
    {
        get { return attempts; }
        set { attempts = value; }
    }

    private bool allPlayersAlive;

    private int roomsCleared = 0;
    public int RoomsCleared
    {
        get { return roomsCleared; }
        set { roomsCleared = value; }
    }
    private int room = 1;
    public int Room
    {
        get { return room; }
        set { room = value; }
    }

    private float difficultyMultiplier = 1;
    [Header("Room Generation")]
    [SerializeField] private GameObject roomGeneration;

    //[Header("Enemy Prefabs")]
    //[SerializeField] private GameObject musketeer;
    //[SerializeField] private GameObject goober;
    //[SerializeField] private GameObject gubGub;
    //[SerializeField] private GameObject devil;

    [SerializeField] private float difficulty;
    public float Difficulty
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

    private int enemyCredits;
    public int EnemyCredits
    {
        get { return enemyCredits; }
        set { enemyCredits = value; }
    }

    private int roomSize;
    public int RoomSize
    {
        get { return roomSize; }
    }

    [SerializeField] private int enemyAmount;
    public int EnemyAmount
    {
        get { return enemyAmount; }
        set { enemyAmount = value; }
    }

    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    private Transform doorPosition;
    public Transform DoorPosition
    {
        get { return doorPosition; }
        set { doorPosition = value; }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Instantiate(roomGeneration, transform);

        attempts = 3;
        health = 10;

        difficulty = (int)DifficultyEnum.baby;
        //Random.InitState(System.DateTime.Now.Millisecond);
        roomSize = Random.Range(0, 3);

        switch (roomSize)
        {
            case (int)RoomSizeEnum.small:
                enemyCredits = (int)MaxEnemyCreditsEnum.small;
                break;
            case (int)RoomSizeEnum.medium:
                enemyCredits = (int)MaxEnemyCreditsEnum.medium;
                break;
            case (int)RoomSizeEnum.large:
                enemyCredits = (int)MaxEnemyCreditsEnum.large;
                break;
        }

        // NewRoom();
    }

    private void Start()
    {
        Debug.Log("Generating room...");
        seed = System.DateTime.Now.Millisecond;
        NewRoom();
    }

    private void Update()
    {
        Mathf.Clamp(difficulty, 0, 100);

        if (attempts == 0)
        {
            SceneManager.LoadScene("Game Over");
        }
        if (health <= 0)
        {
            attempts--;
        }
    }

    public enum RoomSizeEnum
    {
        small = 0, medium = 1, large = 2
    }

    public enum MaxEnemyCreditsEnum
    {
        small = 12, medium = 20, large = 25
    }

    public enum DifficultyEnum
    {
        baby = 40, easy = 70, medium = 90, hard = 100
    }

    public void SetRoomSize()
    {
        roomSize = Random.Range(0, 3);

        switch (roomSize)
        {
            case (int)RoomSizeEnum.small:
                enemyCredits = (int)MaxEnemyCreditsEnum.small;
                break;
            case (int)RoomSizeEnum.medium:
                enemyCredits = (int)MaxEnemyCreditsEnum.medium;
                break;
            case (int)RoomSizeEnum.large:
                enemyCredits = (int)MaxEnemyCreditsEnum.large;
                break;
        }
    }


    public void NewRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //seed = System.DateTime.Now.Millisecond;
            photonView.RPC("CreateRoom", RpcTarget.All, seed);
        }
    }

    [PunRPC]
    void CreateRoom(int newSeed)
    {
        seed = newSeed;
        Instantiate(roomGeneration, transform);
    }

    public void GenerateSeed()
    {
        photonView.RPC("GenerateSeedNet", RpcTarget.All, System.DateTime.Now.Millisecond);
    }

    [PunRPC]
    public void GenerateSeedNet(int newSeed)
    {
        seed = newSeed;
        Debug.Log("GENERATING SEED...");
    }

    public void ResetHealth()
    {
        health = 10;
    }

    public void IncreaseDifficulty()
    {
        room++;
        roomsCleared++;
        difficultyMultiplier += 0.025f;
        difficulty *= difficultyMultiplier;
    }
}
