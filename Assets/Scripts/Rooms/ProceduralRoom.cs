using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProceduralRoom : MonoBehaviour
{
    private List<Colliders> wallListN;
    private List<Colliders> wallListNB;
    private List<Colliders> doorExitList;
    private List<Colliders> doorEnterList;
    private List<Colliders> pillarList;
    private List<Colliders> floorList;

    private List<Colliders> cornerTiles;
    private List<Colliders> upperTiles;
    private List<Colliders> lowerTiles;
    private List<Colliders> leftTiles;
    private List<Colliders> rightTiles;
    private List<Colliders> innerTiles;

    private float spawnPos;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject wallB;
    [SerializeField] private GameObject pillar;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject doorEnter;
    [SerializeField] private GameObject doorExit;

    private GameObject doorInstance;

    [SerializeField] private GameObject[] innerObstaclesPrefab;
    [SerializeField] private GameObject[] cornerObstaclesPrefab;
    [SerializeField] private GameObject[] wallObstaclesPrefab;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private GameObject[] players;
    private GameObject parent;

    [Serializable]
    private struct RoomSize
    {
        public float x;
        public float y;

        public RoomSize(int initialX, int initialY)
        {
            x = initialX;
            y = initialY;
        }
    }

    [SerializeField]
    RoomSize roomSize;

    [Serializable]
    private struct WallSize
    {
        public float x;
        public float y;
    }
    [SerializeField]
    WallSize wallSize;

    [Serializable]
    private struct FloorSize
    {
        public float x;
        public float y;
    }
    [SerializeField]
    FloorSize floorSize;

    private struct Colliders
    {
        public Vector3 pos;
        public Quaternion rot;
    }
    Colliders colliders;

    [Header("Enemy Prefabs")]
    public string musketeer;
    public string goober;
    public string gubGub;
    public string devil;

    private RoomSize smallRoom = new RoomSize(30, 50);
    private RoomSize mediumRoom = new RoomSize(50, 50);
    private RoomSize largeRoom = new RoomSize(80, 50);
    private RoomSize[] rooms = new RoomSize[3];

    private int enemyCredits;
    private int enemyAmount;
    private bool spawnedObstacle;
    private int nextSeed;
    private bool seedGenerated = true;

    void createWalls()
    {
        colliders = new Colliders();
        wallListN = new List<Colliders>();
        wallListNB = new List<Colliders>();
        doorExitList = new List<Colliders>();
        doorEnterList = new List<Colliders>();

        int wallCountX = Mathf.Max(1, (int)(roomSize.x / wallSize.x));
        int wallCountY = Mathf.Max(1, (int)(roomSize.y / wallSize.y));
        float scaleX = (roomSize.x / wallCountX) / wallSize.x;
        float scaleY = (roomSize.y / wallCountY) / wallSize.y;

        //spawnPos = UnityEngine.Random.Range(0, wallCountX);
        spawnPos = 3f;

        // Front walls
        for (int i = 0; i < wallCountX; i++)
        {
            var t = transform.position + new Vector3(-roomSize.x / 2 + wallSize.x * scaleX / 2 + i * scaleX * wallSize.x, -1, -roomSize.y / 2);
            var r = Quaternion.Euler(-90, 180, 0);

            colliders.pos = t;
            colliders.rot = r;

            var rand = UnityEngine.Random.Range(0, 3);

            if (i == (wallCountX - (int)spawnPos))
            {
                Debug.Log("Set spawn in coord: " + i);
                colliders.rot = Quaternion.identity;
                doorEnterList.Add(colliders);
                spawnPosition.transform.position = new Vector3(t.x, t.y, t.z + 4f);
            }
            else if (rand <= 1)
            {
                wallListN.Add(colliders);
            }
            else
            {
                wallListNB.Add(colliders);
            }
        }

        // Back walls
        for (int i = 0; i < wallCountX; i++)
        {
            var t = transform.position + new Vector3(-roomSize.x / 2 + wallSize.x * scaleX / 2 + i * scaleX * wallSize.x, -1, roomSize.y / 2);
            var r = Quaternion.Euler(-90f, 0, 0);

            colliders.pos = t;
            colliders.rot = r;

            var rand = UnityEngine.Random.Range(0, 3);
            if (i == (int)spawnPos)
            {
                Debug.Log("Set spawn in coord: " + i);
                colliders.rot = Quaternion.identity;
                doorExitList.Add(colliders);
            }
            else if (rand <= 1)
            {
                wallListN.Add(colliders);
            }
            else
            {
                wallListNB.Add(colliders);
            }
        }

        // Left Walls
        for (int i = 0; i < wallCountY; i++)
        {
            var t = transform.position + new Vector3(-roomSize.x / 2, -1, -roomSize.y / 2 + wallSize.y * scaleY / 2 + i * scaleY * wallSize.y);
            var r = Quaternion.Euler(-90, 90, 0);

            colliders.pos = t;
            colliders.rot = r;

            var rand = UnityEngine.Random.Range(0, 3);
            if (rand <= 1)
            {
                wallListN.Add(colliders);
            }
            else
            {
                wallListNB.Add(colliders);
            }
        }

        // Right Walls
        for (int i = 0; i < wallCountY; i++)
        {
            var t = transform.position + new Vector3(roomSize.x / 2, -1, -roomSize.y / 2 + wallSize.y * scaleY / 2 + i * scaleY * wallSize.y);
            var r = Quaternion.Euler(-90, 270, 0);

            colliders.pos = t;
            colliders.rot = r;

            var rand = UnityEngine.Random.Range(0, 3);
            if (rand <= 1)
            {
                wallListN.Add(colliders);
            }
            else
            {
                wallListNB.Add(colliders);
            }
        }
    }

    void createPillars()
    {
        colliders = new Colliders();
        pillarList = new List<Colliders>();

        // Bottom Left pillar
        var t = transform.position + new Vector3(-roomSize.x / 2, -1, -roomSize.y / 2);
        var r = Quaternion.Euler(90f, 0, 0);

        colliders.pos = t;
        colliders.rot = r;

        pillarList.Add(colliders);

        // Bottom Right pillar
        t = transform.position + new Vector3(roomSize.x / 2, -1, -roomSize.y / 2);
        r = Quaternion.Euler(90, 270, 0);

        colliders.pos = t;
        colliders.rot = r;

        pillarList.Add(colliders);

        // Top Left pillar
        t = transform.position + new Vector3(-roomSize.x / 2, -1, roomSize.y / 2);
        r = Quaternion.Euler(90, 90, 0);

        colliders.pos = t;
        colliders.rot = r;

        pillarList.Add(colliders);

        // Top Right pillar
        t = transform.position + new Vector3(roomSize.x / 2, -1, roomSize.y / 2);
        r = Quaternion.Euler(90, 180, 0);

        colliders.pos = t;
        colliders.rot = r;

        pillarList.Add(colliders);
    }

    void createFloorTiles()
    {
        floorList = new List<Colliders>();

        cornerTiles = new List<Colliders>();
        upperTiles = new List<Colliders>();
        lowerTiles = new List<Colliders>();
        leftTiles = new List<Colliders>();
        rightTiles = new List<Colliders>();
        innerTiles = new List<Colliders>();

        int floorCountX = Mathf.Max(1, (int)(roomSize.x / floorSize.x));
        int floorCountY = Mathf.Max(1, (int)(roomSize.y / floorSize.y));
        float scaleX = (roomSize.x / floorCountX) / floorSize.x;
        float scaleY = (roomSize.y / floorCountY) / floorSize.y;

        for (int i = 0; i < floorCountX; i++)
        {
            for (int j = 0; j < floorCountY; j++)
            {

                var t = transform.position + new Vector3(-roomSize.x / 2 + floorSize.x * scaleX / 2 + i * scaleX * floorSize.x, -1, -roomSize.y / 2 + floorSize.y * scaleY / 2 + j * scaleY * floorSize.y);
                var r = Quaternion.Euler(90, 0, 0);

                colliders.pos = t;
                colliders.rot = r;

                floorList.Add(colliders);

                // Calculate tile position
                // Upper corners
                if (i == 0 && (j == 0 || j == floorCountY - 1))
                {
                    cornerTiles.Add(colliders);
                }
                // Lower corners
                else if (i == floorCountX - 1 && (j == 0 || j == floorCountY - 1))
                {
                    cornerTiles.Add(colliders);
                }
                // Upper tiles
                else if (i == 0)
                {
                    upperTiles.Add(colliders);
                }
                // Left tiles
                else if (j == 0)
                {
                    leftTiles.Add(colliders);
                }
                // Lower tiles
                else if (i == floorCountX - 1)
                {
                    lowerTiles.Add(colliders);
                }
                // Right tiles
                else if (j == floorCountY - 1)
                {
                    rightTiles.Add(colliders);
                }
                // Inner tiles
                else
                {
                    innerTiles.Add(colliders);
                }
            }
        }
    }

    void renderWalls()
    {
        for (int i = 0; i < wallListN.Count; i++)
        {
            Instantiate(wall, wallListN[i].pos, wallListN[i].rot, parent.transform);
        }

        for (int i = 0; i < wallListNB.Count; i++)
        {
            Instantiate(wallB, wallListNB[i].pos, wallListNB[i].rot, parent.transform);
        }

        Instantiate(doorEnter, doorEnterList[0].pos, doorEnterList[0].rot, parent.transform);
        doorInstance = Instantiate(doorExit, doorExitList[0].pos, doorEnterList[0].rot, parent.transform);
        GameManager.Instance.DoorPosition = doorInstance.transform;
        doorInstance.GetComponent<BoxCollider>().enabled = false;
    }

    void renderPillars()
    {
        for (int i = 0; i < pillarList.Count; i++)
        {
            Instantiate(pillar, pillarList[i].pos, pillarList[i].rot, parent.transform);
        }
    }

    void renderFloorTiles()
    {
        for (int i = 0; i < floorList.Count; i++)
        {
            Instantiate(floorTile, floorList[i].pos, floorList[i].rot, parent.transform);
        }
    }

    void generateObstacles()
    {
        for (int i = 0; i < cornerTiles.Count; i++)
        {
            GameObject prefab = cornerObstaclesPrefab[UnityEngine.Random.Range(0, cornerObstaclesPrefab.Length)];
            GameObject obstacle = Instantiate(prefab, parent.transform);
            obstacle.transform.position = cornerTiles[i].pos;
            obstacle.transform.rotation = Quaternion.Euler(0, 0, 0);
            obstacle.transform.localScale = Vector3.one;
        }

        for (int i = 0; i < upperTiles.Count; i++)
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            if (chance <= 50f)
            {
                GameObject prefab = wallObstaclesPrefab[UnityEngine.Random.Range(0, wallObstaclesPrefab.Length)];
                GameObject obstacle = Instantiate(prefab, parent.transform);
                obstacle.transform.position = upperTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, -90, 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);
            }
        }

        for (int i = 0; i < leftTiles.Count; i++)
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            if (i == (int)spawnPos)
            {
                //spawnPositions[0].transform.position = lowerTiles[i].pos;
                //spawnPosition.transform.position = leftTiles[i].pos;
                Debug.Log("Set spawn position in coord: " + i);
            }
            else if (chance <= 50f)
            {
                GameObject prefab = wallObstaclesPrefab[UnityEngine.Random.Range(0, wallObstaclesPrefab.Length)];
                GameObject obstacle = Instantiate(prefab, parent.transform);
                obstacle.transform.position = leftTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, 180, 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);
            }
        }

        for (int i = 0; i < lowerTiles.Count; i++)
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            if (chance <= 50f)
            {
                GameObject prefab = wallObstaclesPrefab[UnityEngine.Random.Range(0, wallObstaclesPrefab.Length)];
                GameObject obstacle = Instantiate(prefab, parent.transform);
                obstacle.transform.position = lowerTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, 90, 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);

            }
        }

        for (int i = 0; i < rightTiles.Count; i++)
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            if (chance <= 50f)
            {
                GameObject prefab = wallObstaclesPrefab[UnityEngine.Random.Range(0, wallObstaclesPrefab.Length)];
                GameObject obstacle = Instantiate(prefab, parent.transform);
                obstacle.transform.position = rightTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, 0, 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);
            }
        }

        for (int i = 0; i < innerTiles.Count; i++)
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            if (chance <= 40f)
            {
                GameObject prefab = innerObstaclesPrefab[UnityEngine.Random.Range(0, innerObstaclesPrefab.Length)];
                GameObject obstacle = Instantiate(prefab, parent.transform);
                obstacle.transform.position = innerTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);
                spawnedObstacle = true;
            }
            //else if(chance <= 50f && enemyCredits!=0)
            if (PhotonNetwork.IsMasterClient)
            {
                chance = UnityEngine.Random.Range(0f, 100f);
                if (chance <= 65f && enemyCredits != 0 && !spawnedObstacle)
                {
                    chance = UnityEngine.Random.Range(0f, GameManager.Instance.Difficulty);
                    if ((chance > 90f && chance <= 100f) && enemyCredits % 4 == 0)
                    {
                        GameObject devilObject = PhotonNetwork.Instantiate(devil, Vector3.zero, Quaternion.identity);
                        devilObject.transform.position = new Vector3(innerTiles[i].pos.x, 0, innerTiles[i].pos.z);
                        enemyCredits -= 4;
                        enemyAmount++;
                    }
                    else if ((chance > 70f && chance <= 90f) && enemyCredits % 3 == 0)
                    {
                        GameObject musketeerObject = PhotonNetwork.Instantiate(musketeer, Vector3.zero, Quaternion.identity);
                        musketeerObject.transform.position = new Vector3(innerTiles[i].pos.x, 0, innerTiles[i].pos.z);
                        enemyCredits -= 3;
                        enemyAmount++;
                    }
                    else if ((chance > 40f && chance <= 70f) && enemyCredits % 2 == 0)
                    {
                        GameObject gubgubObject = PhotonNetwork.Instantiate(gubGub, Vector3.zero, Quaternion.identity);
                        gubgubObject.transform.position = new Vector3(innerTiles[i].pos.x, 0, innerTiles[i].pos.z);
                        enemyCredits -= 2;
                        enemyAmount++;
                    }
                    else if (chance <= 40f && enemyCredits != 0)
                    {
                        GameObject gooberObject = PhotonNetwork.Instantiate(goober, Vector3.zero, Quaternion.identity);
                        gooberObject.transform.position = new Vector3(innerTiles[i].pos.x, 0, innerTiles[i].pos.z);
                        enemyCredits -= 1;
                        enemyAmount++;
                    }
                }
            }
            spawnedObstacle = false;
        }
        GameManager.Instance.EnemyAmount = enemyAmount;
    }

    private void Start()
    {
        rooms[0] = smallRoom;
        rooms[1] = mediumRoom;
        rooms[2] = largeRoom;
        
        generateRoom(GameManager.Instance.seed);
        // GameManager.Instance.seed = GameManager.Instance.seed + System.DateTime.Now.Millisecond;
    }

    public void generateRoom(int seed)
    { 
        if(parent != null)
        {
            Destroy(parent);
        }
        parent = new GameObject("Generation");
        parent.transform.SetParent(transform);
        UnityEngine.Random.InitState(seed);
        enemyAmount = 0;
        GameManager.Instance.SetRoomSize();
        roomSize = rooms[GameManager.Instance.RoomSize];
        enemyCredits = GameManager.Instance.EnemyCredits;
        createWalls();
        createPillars();
        createFloorTiles();

        renderWalls();
        renderPillars();
        renderFloorTiles();

        generateObstacles();
        Debug.Log("Looking for players");
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = spawnPosition.transform.GetChild(i).position;
        }
    }

    public enum EnemyValues
    {
        goober = 1, gubGub = 2, musketeer = 3, devil = 4
    }

    public void Update()
    {
        if (GameManager.Instance.EnemyAmount == 0)
        {
            doorInstance.GetComponent<BoxCollider>().enabled = true;
            if (PhotonNetwork.IsMasterClient && !seedGenerated)
            {
                GameManager.Instance.GenerateSeed();
                seedGenerated = true;
                //GameManager.Instance.seed = System.DateTime.Now.Millisecond;
                //seedGenerated = false;
            }
        }

        if (GameManager.Instance.Health <= 0)
        {
            GameManager.Instance.ResetHealth();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].transform.position = spawnPosition.transform.GetChild(i).position;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.IncreaseDifficulty();
            generateRoom(GameManager.Instance.seed);
            seedGenerated = true;
            
        }
    }

}
