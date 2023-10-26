using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRoom : MonoBehaviour
{
    private List<Colliders> wallListN;
    private List<Colliders> wallListNB;
    private List<Colliders> pillarList;
    private List<Colliders> floorList;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject wallB;
    [SerializeField] private GameObject pillar;
    [SerializeField] private GameObject floorTile;

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

        int floorCountX = Mathf.Max(1, (int)(roomSize.x / floorSize.x));
        int floorCountY = Mathf.Max(1, (int)(roomSize.y / floorSize.y));
        float scaleX = (roomSize.x / floorCountX) / floorSize.x;
        float scaleY = (roomSize.y / floorCountY) / floorSize.y;

        for (int i = 0; i < floorCountX; i++)
        {
            for (int j = 0; j < floorCountY; j++) 
            {
                var t = transform.position + new Vector3(-roomSize.x / 2 + floorSize.x * scaleX / 2 + i * scaleX * floorSize.x, 0, -roomSize.y / 2 + floorSize.y * scaleY / 2 + j * scaleY * floorSize.y);
                var r = transform.rotation;
                
                colliders.pos = t;
                colliders.rot = r;

                floorList.Add(colliders);
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

    private void Start()
    {
        createWalls();
        createPillars();
        createFloorTiles();

        renderWalls();
        renderPillars();
        renderFloorTiles();
    }
}
