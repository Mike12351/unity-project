using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    //chunkprefabs array stores all the possible prefabs that can be instantiated
    public GameObject[] chunkPrefabs;

    public GameObject[] EasyChunkPrefabs;

    //playerTransform is the player controller
    private Transform playerTransform;

    //reusable variables for ease of computations
    private int chunkLength = 20;
    private int arrayLength;
    private bool movedChunk = false;
    //4x4 cube around the player
    public int maxChunks = 4;

    // previous and new players coordinates, used to find out whether a player has left and entered a new chunk
    private int previous_playerX = 0; 
    private int previous_playerZ = 0;
    private int new_playerX = 0;
    private int new_playerZ = 0;

    //Dictionary to stores data about the current maze
    //Keys are the coordinates on where the chunk is placed on the xz axis
    //value is of type Vector3?4 to store exact coordinates of the chunk and rotation and type of chunk
    private Dictionary<Vector2, GameObject> loadedChunks = new Dictionary<Vector2, GameObject>();
    //deletedChunks stores information about the chunks that have been deleted so that they can be respawned at their old location with the same properties
    private Dictionary<Vector2, Vector3> deletedChunks = new Dictionary<Vector2, Vector3>();
    //type chunks stores the type of chunks for each coordinate, used when respawning deleted chunks
    private Dictionary<Vector2, Vector2> typeChunks = new Dictionary<Vector2, Vector2>();

    public DifficultyManager dm;

    private int difficulty = 2;

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
            movedChunk = true;
            //dm.setNewChunk();
        }else if (previous_playerZ != new_playerZ)
        {
            movedChunkZ(previous_playerZ, new_playerZ);
            previous_playerZ = new_playerZ;
            movedChunk = true;
            //dm.setNewChunk();
        }

        //if the player has moved to different chunk, check if we need to spawn new chunks or delete some chunks
        if (movedChunk)
        {
            List<Vector2> toRemove = new List<Vector2>();
            //if the player has moved chunks
            //remove all chunks currently loaded that are further away then the maxChunks
            //store the deleted chunks in the a new array incase we have to respawn them at the same positions
            foreach (var chunk in loadedChunks)
            {
                if (chunk.Key.x > new_playerX + maxChunks || chunk.Key.x < new_playerX - maxChunks)
                {
                    //new Vector3(go.transform.position.x, go.transform.position.z, go.transform.rotation.y)
                    if (!deletedChunks.ContainsKey(chunk.Key))
                    {
                        deletedChunks.Add(chunk.Key, new Vector3(chunk.Value.transform.position.x, chunk.Value.transform.position.z, chunk.Value.transform.eulerAngles.y));
                        toRemove.Add(chunk.Key);
                    }
                }

                if (chunk.Key.y > new_playerZ + maxChunks || chunk.Key.y < new_playerZ - maxChunks)
                {
                    //new Vector3(go.transform.position.x, go.transform.position.z, go.transform.rotation.y)
                    if (!deletedChunks.ContainsKey(chunk.Key))
                    {
                        deletedChunks.Add(chunk.Key, new Vector3(chunk.Value.transform.position.x, chunk.Value.transform.position.z, chunk.Value.transform.eulerAngles.y));
                        toRemove.Add(chunk.Key);
                    }
                }
            }

            foreach (var k in toRemove)
            {
                Destroy(loadedChunks[k]);
                loadedChunks.Remove(k);
            }
            toRemove.Clear();
        }
    }

    //spawns a new chunk at the given location, with random orientation and random type
    private void spawnChunk(float x, float z, int prefabIndex = -1)
    {
        GameObject go;
        int randSpawnIndex;
        int randRotIndex;
        System.Random rnd = new System.Random();

        GameObject[] temparr;

        if (difficulty == 2)
        {
            temparr = chunkPrefabs;
        }
        else
        {
            temparr = EasyChunkPrefabs;
        }

        arrayLength = temparr.Length;

        randSpawnIndex = rnd.Next(1, arrayLength);
        if (x == 0 && x == z)
        {
            randSpawnIndex = 0;
            go = Instantiate(temparr[0]) as GameObject;
        }
        else
        {
            go = Instantiate(temparr[randSpawnIndex]) as GameObject;
        }
        go.transform.SetParent(transform);

        randRotIndex = rnd.Next(0, 4);
        Vector2 rotVec = rotateChunk(randRotIndex);

        go.transform.rotation = Quaternion.Euler(0f, 90 * randRotIndex, 0f);
        go.transform.position = Vector3.right * x * chunkLength + Vector3.forward * z * chunkLength;
        go.transform.position += new Vector3(rotVec.x, 0f, rotVec.y);

        loadedChunks.Add(new Vector2(x, z),go);
        typeChunks.Add(new Vector2(x, z), new Vector2(randSpawnIndex,difficulty));

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
        List<Vector2> toRemove = new List<Vector2>();
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
                //check if the chunk was deleted before
                if (!deletedChunks.ContainsKey(new Vector2(spawnX, spawnZ)))
                {
                    spawnChunk(spawnX, spawnZ);
                }
                else
                {
                    //chunk was deleted before
                    respawnChunk(spawnX, spawnZ, deletedChunks[new Vector2(spawnX, spawnZ)], typeChunks[new Vector2(spawnX, spawnZ)]);
                    //remove from deleted chunk since its loaded now and add it to loaded chunks
                    toRemove.Add(new Vector2(spawnX, spawnZ));
                }
            }
        }

        foreach (var k in toRemove)
        {
            deletedChunks.Remove(k);
        }
        toRemove.Clear();
    }

    //when moving in the z axis create new chunks at the edge if not already created
    private void movedChunkZ(int previousZ, int newZ)
    {
        int spawnX;
        int spawnZ;
        List<Vector2> toRemove = new List<Vector2>();
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
                //check if the chunk was deleted before
                if (!deletedChunks.ContainsKey(new Vector2(spawnX, spawnZ)))
                {
                    spawnChunk(spawnX, spawnZ);
                }
                else
                {
                    //chunk was deleted before
                    respawnChunk(spawnX, spawnZ, deletedChunks[new Vector2(spawnX, spawnZ)], typeChunks[new Vector2(spawnX, spawnZ)]);
                    //remove from deleted chunk since its loaded now and add it to loaded chunks
                    toRemove.Add(new Vector2(spawnX, spawnZ));
                }
            }
        }

        foreach (var k in toRemove)
        {
            deletedChunks.Remove(k);
        }
        toRemove.Clear();
    }

    //function used to respawn an existing chunk
    private void respawnChunk(float x, float z, Vector3 data, Vector2 type)
    {
        GameObject go;
        int randSpawnIndex = (int) type.x;
        int typeDiff = (int)type.y;

        GameObject[] temparr;
        if (typeDiff == 2)
        {
            temparr = chunkPrefabs;
        }
        else
        {
            temparr = EasyChunkPrefabs;
        }
        go = Instantiate(temparr[randSpawnIndex]) as GameObject;
        go.transform.SetParent(transform);

        go.transform.eulerAngles = new Vector3(0f, data.z, 0f);
        go.transform.position = Vector3.right * data.x + Vector3.forward * data.y;

        loadedChunks.Add(new Vector2(x, z), go);
    }

    public void decreaseDiff()
    {
        print("diff decreased");
        dm.resetTimer();
        difficulty = 1;
    }
}
