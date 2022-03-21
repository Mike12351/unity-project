using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    //chunkprefabs array stores all the possible prefabs that can be instantiated
    public GameObject[] chunkPrefabs;

    //playerTransform is the player controller
    private Transform playerTransform;

    //reusable variables for ease of computations
    private int chunkLength = 20;
    private int arrayLength;

    // previous and new players coordinates, used to find out whether a player has left and entered a new chunk
    private int previous_playerX = 0; 
    private int previous_playerZ = 0;
    private int new_playerX = 0;
    private int new_playerZ = 0;

    //Dictionary to stores data about the current maze
    //Keys are the coordinates on where the chunk is placed on the xz axis
    //value is of type Vector3?4 to store exact coordinates of the chunk and rotation and type of chunk
    private Dictionary<Vector2, Vector3> loadedChunks = new Dictionary<Vector2, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        arrayLength = chunkPrefabs.Length;

        initChunks();
    }

    // Update is called once per frame
    void Update()
    {
        //check if player has left/entered a new chunk
        if (playerTransform.position.x < 0)
        {
            new_playerX = (int)(playerTransform.position.x - chunkLength) / chunkLength;
        }
        else
        {
            new_playerX = (int)(playerTransform.position.x) / chunkLength;
        }
        if (playerTransform.position.z > 0)
        {
            new_playerZ = (int)(playerTransform.position.z + chunkLength) / chunkLength;
        }
        else
        {
            new_playerZ = (int)(playerTransform.position.z) / chunkLength;
        }

        //if entered a new chunk, check which axis and which direction and call the spawn new chunk functions
        if (previous_playerX != new_playerX)
        {
            movedChunkX(previous_playerX, new_playerX);
            previous_playerX = new_playerX;
        }else if (previous_playerZ != new_playerZ)
        {
            movedChunkZ(previous_playerZ, new_playerZ);
            previous_playerZ = new_playerZ;
        }
    }

    //spawns a new chunk at the given location, with random orientation and random type
    private void spawnChunk(float x, float z, int prefabIndex = -1)
    {
        GameObject go;
        int randSpawnIndex;
        int randRotIndex;
        System.Random rnd = new System.Random();
        if (x == 0 && x == z)
        {
            go = Instantiate(chunkPrefabs[0]) as GameObject;
        }
        else
        {
            randSpawnIndex = rnd.Next(1,arrayLength);
            go = Instantiate(chunkPrefabs[randSpawnIndex]) as GameObject;
        }
        go.transform.SetParent(transform);

        randRotIndex = rnd.Next(0, 4);
        Vector2 rotVec = rotateChunk(randRotIndex);

        go.transform.rotation = Quaternion.Euler(0f, 90 * randRotIndex, 0f);
        go.transform.position = Vector3.right * x * chunkLength + Vector3.forward * z * chunkLength;
        go.transform.position += new Vector3(rotVec.x, 0f, rotVec.y);

        loadedChunks.Add(new Vector2(x, z), new Vector3(go.transform.position.x,go.transform.position.z,go.transform.rotation.y));

    }

    //inital fucntion to set up the base maze 5x5
    private void initChunks()
    {

        for (int x = -2; x <= 2; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                spawnChunk(x,z);
            }
        }
    }

    //function used to help with the orientation of the chunks
    private Vector2 rotateChunk(int index)
    {
        if (index == 0)
        {
            return new Vector2(0, 0);
        }else if (index == 1)
        {
            return new Vector2(chunkLength, 0);
        }else if (index == 2)
        {
            return new Vector2(chunkLength, -chunkLength);
        }
        else
        {
            return new Vector2(0, -chunkLength);
        }
    }

    //when moving in the x axis create new chunks at the edge if not already created
    private void movedChunkX(int previousX, int newX)
    {
        int spawnX;
        int spawnZ;

        if (newX - previousX ==1)
        {
            spawnX = newX + 2;
        }
        else
        {
            spawnX = newX - 2;
        }

        for (int i = -2; i<=2; i++)
        {
            spawnZ = new_playerZ + i;
            if (!loadedChunks.ContainsKey(new Vector2(spawnX, spawnZ)))
            {
                spawnChunk(spawnX, spawnZ);
            }
        }
    }

    //when moving in the z axis create new chunks at the edge if not already created
    private void movedChunkZ(int previousZ, int newZ)
    {
        int spawnX;
        int spawnZ;

        if (newZ - previousZ == 1)
        {
            spawnZ = newZ + 2;
        }
        else
        {
            spawnZ = newZ - 2;
        }

        for (int i = -2; i <= 2; i++)
        {
            spawnX = new_playerX + i;
            if (!loadedChunks.ContainsKey(new Vector2(spawnX, spawnZ)))
            {
                spawnChunk(spawnX, spawnZ);
            }
        }
    }
}
