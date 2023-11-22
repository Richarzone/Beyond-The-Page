using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int attempts = 3;
    private int enemyAmount;
    private bool allPlayersAlive;
    private int roomsCleared;
    private int room = 0;

    [Header("Room Generation")]
    [SerializeField] private GameObject roomGeneration;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject musketeer;
    [SerializeField] private GameObject goober;
    [SerializeField] private GameObject gubGub;
    [SerializeField] private GameObject devil;

    [SerializeField] private float spawnHeight;

    private float difficulty;
    private float roomSize;
    private float creditConstant;
    private float creditNumber;
    private float gooberWeight;
    private float gubgubWeight;
    private float musketeerWeight;
    private float devilWeight;
    private float gooberPercent;
    private float gubgubPercent;
    private float musketeerPercent;
    private float devilPercent;

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
    }

    private void Start()
    {
        
    }

    public enum MaxEnemyCredits
    {
        small = 12, medium = 25, large = 35 
    }

    public enum EnemyValues
    {
        goober = 1, gubGub = 2, musketeer = 3, devil = 4
    }

    public enum Difficulty
    {
        baby = 40, easy = 70, medium = 90, hard = 100
    }
}
