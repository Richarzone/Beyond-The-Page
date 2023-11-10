using System;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRoom : MonoBehaviour
{
    private List<Colliders> wallListN;
    private List<Colliders> wallListNB;
    private List<Colliders> pillarList;
    private List<Colliders> floorList;
    
    private List<Colliders> cornerTiles;
    private List<Colliders> upperTiles;
    private List<Colliders> lowerTiles;
    private List<Colliders> leftTiles;
    private List<Colliders> rightTiles;
    private List<Colliders> innerTiles;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject wallB;
    [SerializeField] private GameObject pillar;
    [SerializeField] private GameObject floorTile;

    [SerializeField] private GameObject[] innerObstaclesPrefab;
    [SerializeField] private GameObject[] cornerObstaclesPrefab;
    [SerializeField] private GameObject[] wallObstaclesPrefab;

    [Serializable]
    private struct RoomSize
    {
        public float x;
        public float y;
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

    void createWalls()
    {
        UnityEngine.Random.InitState(42);

        colliders = new Colliders();
        wallListN = new List<Colliders>();
        wallListNB = new List<Colliders>();

        int wallCountX = Mathf.Max(1, (int)(roomSize.x / wallSize.x));
        int wallCountY = Mathf.Max(1, (int)(roomSize.y / wallSize.y));
        float scaleX = (roomSize.x / wallCountX) / wallSize.x;
        float scaleY = (roomSize.y / wallCountY) / wallSize.y;

        
        // Front walls
        for (int i = 0; i < wallCountX; i++)
        {
            var t = transform.position + new Vector3(-roomSize.x / 2 + wallSize.x * scaleX / 2 + i * scaleX * wallSize.x, 0, -roomSize.y / 2);
            var r = Quaternion.Euler(0, 180, 0);

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
        
        // Back walls
        for (int i = 0; i < wallCountX; i++)
        {
            var t = transform.position + new Vector3(-roomSize.x / 2 + wallSize.x * scaleX / 2 + i * scaleX * wallSize.x, 0, roomSize.y / 2);
            var r = transform.rotation;

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

        // Left Walls
        for (int i = 0; i < wallCountY; i++)
        {
            var t = transform.position + new Vector3(-roomSize.x / 2, 0, -roomSize.y / 2 + wallSize.y * scaleY / 2 + i * scaleY * wallSize.y);
            var r = Quaternion.Euler(0, 90, 0);

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
            var t = transform.position + new Vector3(roomSize.x / 2, 0, -roomSize.y / 2 + wallSize.y * scaleY / 2 + i * scaleY * wallSize.y);
            var r = Quaternion.Euler(0, 270, 0);

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
        var t = transform.position + new Vector3(-roomSize.x / 2, 0, -roomSize.y / 2);
        var r = transform.rotation;

        colliders.pos = t;
        colliders.rot = r;

        pillarList.Add(colliders);

        // Bottom Right pillar
        t = transform.position + new Vector3(roomSize.x / 2, 0, -roomSize.y / 2);
        r = Quaternion.Euler(0, 270, 0);

        colliders.pos = t;
        colliders.rot = r;

        pillarList.Add(colliders);

        // Top Left pillar
        t = transform.position + new Vector3(-roomSize.x / 2, 0, roomSize.y / 2);
        r = Quaternion.Euler(0, 90, 0);

        colliders.pos = t;
        colliders.rot = r;

        pillarList.Add(colliders);

        // Top Right pillar
        t = transform.position + new Vector3(roomSize.x / 2, 0, roomSize.y / 2);
        r = Quaternion.Euler(0, 180, 0);

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

                var t = transform.position + new Vector3(-roomSize.x / 2 + floorSize.x * scaleX / 2 + i * scaleX * floorSize.x, 0, -roomSize.y / 2 + floorSize.y * scaleY / 2 + j * scaleY * floorSize.y);
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
            Instantiate(wall, wallListN[i].pos, wallListN[i].rot);
        }

        for (int i = 0; i < wallListNB.Count; i++)
        {
            Instantiate(wallB, wallListNB[i].pos, wallListNB[i].rot);
        }
    }

        void renderPillars()
    {
        for (int i = 0; i < pillarList.Count; i++)
        {
            Instantiate(pillar, pillarList[i].pos, pillarList[i].rot);
        }
    }

    void renderFloorTiles()
    {
        for (int i = 0; i < floorList.Count; i++)
        {
            Instantiate(floorTile, floorList[i].pos, floorList[i].rot);
        }
    }

    void generateObstacles()
    {
        for (int i = 0; i < cornerTiles.Count; i++)
        {
            GameObject prefab = cornerObstaclesPrefab[UnityEngine.Random.Range(0, cornerObstaclesPrefab.Length)];
            GameObject obstacle = Instantiate(prefab, transform);
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
                GameObject obstacle = Instantiate(prefab, transform);
                obstacle.transform.position = upperTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, -90, 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);
            }
        }

        for (int i = 0; i < leftTiles.Count; i++)
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            if (chance <= 50f)
            {
                GameObject prefab = wallObstaclesPrefab[UnityEngine.Random.Range(0, wallObstaclesPrefab.Length)];
                GameObject obstacle = Instantiate(prefab, transform);
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
                GameObject obstacle = Instantiate(prefab, transform);
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
                GameObject obstacle = Instantiate(prefab, transform);
                obstacle.transform.position = rightTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, 0, 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);
            }
        }

        for (int i = 0; i < innerTiles.Count; i++)
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            if (chance <= 50f)
            {
                GameObject prefab = innerObstaclesPrefab[UnityEngine.Random.Range(0, innerObstaclesPrefab.Length)];
                GameObject obstacle = Instantiate(prefab, transform);
                obstacle.transform.position = innerTiles[i].pos;
                obstacle.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0);
                obstacle.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.3f, 0.6f);
            }
        }
    }

    private void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        createWalls();
        createPillars();
        createFloorTiles();

        renderWalls();
        renderPillars();
        renderFloorTiles();

        generateObstacles();
    }
}
