using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int attempts = 3;
    
    private bool allPlayersAlive;
    private int roomsCleared;
    private int room = 0;

    [Header("Room Generation")]
    [SerializeField] private GameObject roomGeneration;

    //[Header("Enemy Prefabs")]
    //[SerializeField] private GameObject musketeer;
    //[SerializeField] private GameObject goober;
    //[SerializeField] private GameObject gubGub;
    //[SerializeField] private GameObject devil;

    [SerializeField] private int difficulty;
    public int Difficulty
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

        Instantiate(roomGeneration, transform);

        //difficulty = (int)DifficultyEnum.baby;
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
    }

    private void Start()
    {
    }

    private void Update()
    {
        //if (playerHealth <= 0)
        //{

        //}

        if (enemyAmount == 0)
        {
            GetComponentInChildren<ProceduralRoom>().generateRoom();
        }
    }

    public enum RoomSizeEnum
    {
        small = 0, medium = 1, large = 2
    }

    public enum MaxEnemyCreditsEnum
    {
        small = 12, medium = 25, large = 35
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
}
