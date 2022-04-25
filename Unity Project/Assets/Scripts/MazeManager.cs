using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    //playerTransform is the player controller
    private Transform playerTransform;

    //settings of the maze manager
    private const int chunkLength = 20;
    private const int initialLength = 4;

    //prefabs arrays
    public GameObject[] EasyChunkPrefabs;
    public GameObject[] MediumChunkPrefabs;
    public GameObject[] HardChunkPrefabs;
    public GameObject[] special;

    //Dictionary to stores data about the current maze
    //Keys are the coordinates on where the chunk is placed on the xz axis
    //value is of type Gameobject to store the chunk
    private Dictionary<Vector2, GameObject> activeChunks = new Dictionary<Vector2, GameObject>();

    //deletedChunks stores information about the chunks that have been deleted so that they can be respawned at their old location with the same properties
    //Keys are the coordinates on where the chunk was placed on the xz axis
    //value is of type Vectore3 to store the information of the chunks such as the x and z of the chunk transform and the euler angles of the chunk
    private Dictionary<Vector2, Vector3> deletedChunks = new Dictionary<Vector2, Vector3>();

    //type chunks stores the type of chunks for each coordinate, used when respawning deleted chunks
    //Keys are the coordinates on where the chunk was placed on the xz axis
    //value is of type Vectore2 to store the information of the chunks as the prefab index and the difficulty
    private Dictionary<Vector2, Vector2> typeChunks = new Dictionary<Vector2, Vector2>();

    //variables used in computations

    //difficulty of the maze -> 1:Easy; 2:Medium; 3:Hard
    public DifficultyManager dm;
    private int difficulty;
    //arrayLength stores the current length of the array of the respective difficulty array
    private int arrayLength;
    // previous and new players coordinates, used to find out whether a player has left and entered a new chunk
    private int previous_playerX = 0;
    private int previous_playerZ = 0;
    private int new_playerX = 0;
    private int new_playerZ = 0;
    //check if the player has moved from a chunk
    private bool movedChunk = false;

    // Start is called before the first frame update
    void Start()
    {
        //set the initial values of the maze
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        difficulty = 2;
        arrayLength = MediumChunkPrefabs.Length;

        //create the initial maze
        initMaze();
        initGoal();
    }

    //change the difficulty system so that its not all easy/medium or hard chunks but instead create a system that starts at 33% for each chunk and depending on the difficulty scale we adjust the rates of which chunks we should spawn

    // Update is called once per frame
    void Update()
    {
        //check if player has moved the the chunk it is currently on
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
            dm.enteredNewChunk();
        }
        else if (previous_playerZ != new_playerZ)
        {
            movedChunkZ(previous_playerZ, new_playerZ);
            previous_playerZ = new_playerZ;
            movedChunk = true;
            dm.enteredNewChunk();
        }

        //if the player has moved to different chunk, check if we need to spawn new chunks or delete some chunks
        if (movedChunk)
        {
            //temporary list to store which chunks to remove from active chunks at the end of the for loop
            List<Vector2> toRemove = new List<Vector2>();
            //if the player has moved chunks
            //remove all chunks currently loaded that are further away than the initialLength value
            //store the deleted chunks in the a new array incase we have to respawn them in the same positions
            foreach (var chunk in activeChunks)
            {
                if (chunk.Key.x > new_playerX + initialLength || chunk.Key.x < new_playerX - initialLength)
                {
                    if (!deletedChunks.ContainsKey(chunk.Key))
                    {
                        deletedChunks.Add(chunk.Key, new Vector3(chunk.Value.transform.position.x, chunk.Value.transform.position.z, chunk.Value.transform.eulerAngles.y));
                        toRemove.Add(chunk.Key);
                    }
                }

                if (chunk.Key.y > new_playerZ + initialLength || chunk.Key.y < new_playerZ - initialLength)
                {
                    //new Vector3(go.transform.position.x, go.transform.position.z, go.transform.rotation.y)
                    if (!deletedChunks.ContainsKey(chunk.Key))
                    {
                        deletedChunks.Add(chunk.Key, new Vector3(chunk.Value.transform.position.x, chunk.Value.transform.position.z, chunk.Value.transform.eulerAngles.y));
                        toRemove.Add(chunk.Key);
                    }
                }
            }

            //remove chunks in question
            foreach (var k in toRemove)
            {
                Destroy(activeChunks[k]);
                activeChunks.Remove(k);
            }
            toRemove.Clear();
        }
    }

    private void initMaze()
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                spawnChunk(x, z,-1,true);
            }
        }
    }

    private void initGoal()
    {
        System.Random rnd = new System.Random();
        GameObject go;
        int x, z;

        x = rnd.Next(3, 5);
        z = rnd.Next(3, 5);

        go = Instantiate(special[1]) as GameObject;

        go.transform.SetParent(transform);

        go.transform.position = Vector3.right * x * chunkLength + Vector3.forward * z * chunkLength;

        activeChunks.Add(new Vector2(x, z), go);
        typeChunks.Add(new Vector2(x, z), new Vector2(1, 4));
    }

    //spawns a new chunk at the given location, with random orientation and random type
    private void spawnChunk(float x, float z, int prefabIndex = -1, bool randomize = false)
    {
        GameObject go;
        System.Random rnd = new System.Random();
        //random prefabIndex and random rotation
        int randSpawnIndex;
        int randRotIndex;
        int diff;

        if (randomize)
        {
            diff = rnd.Next(1, 4);
        }
        else
        {
            diff = difficulty;
        }

        if (x == 0 && x == z)
        {
            randSpawnIndex = 0;
            diff = 4;
            go = Instantiate(special[randSpawnIndex]) as GameObject;
        }
        else
        {
            if (diff == 1)
            {
                arrayLength = EasyChunkPrefabs.Length;
                randSpawnIndex = rnd.Next(0, arrayLength);
                go = Instantiate(EasyChunkPrefabs[randSpawnIndex]) as GameObject;
            }
            else if (diff == 2)
            {
                arrayLength = MediumChunkPrefabs.Length;
                randSpawnIndex = rnd.Next(0, arrayLength);
                go = Instantiate(MediumChunkPrefabs[randSpawnIndex]) as GameObject;
            }else
            {
                arrayLength = HardChunkPrefabs.Length;
                randSpawnIndex = rnd.Next(0, arrayLength);
                go = Instantiate(HardChunkPrefabs[randSpawnIndex]) as GameObject;
            }
        }

        go.transform.SetParent(transform);

        randRotIndex = rnd.Next(0, 4);
        Vector2 rotVec = rotateChunk(randRotIndex);

        go.transform.rotation = Quaternion.Euler(0f, 90 * randRotIndex, 0f);
        go.transform.position = Vector3.right * x * chunkLength + Vector3.forward * z * chunkLength;
        go.transform.position += new Vector3(rotVec.x, 0f, rotVec.y);

        activeChunks.Add(new Vector2(x, z), go);
        typeChunks.Add(new Vector2(x, z), new Vector2(randSpawnIndex, diff));
    }

    //helper function for the rotation of the chunks
    private Vector2 rotateChunk(int index)
    {
        if (index == 0)
        {
            return new Vector2(0, 0);
        }
        else if (index == 1)
        {
            return new Vector2(chunkLength, 0);
        }
        else if (index == 2)
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
        if (newX - previousX == 1)
        {
            spawnX = newX + 2;
        }
        else
        {
            spawnX = newX - 2;
        }

        for (int i = -2; i <= 2; i++)
        {
            spawnZ = new_playerZ + i;
            //since we moved in the x axis, we now check for all possible new spawn location whether they are active and if they are not meaning there is a free space
            //we then check whether it has been deleted before or not so we can then either create a new chunk or respawn an old chunk with the old properties
            //same method used in the movedChunkZ function
            if (!activeChunks.ContainsKey(new Vector2(spawnX, spawnZ)))
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
            if (!activeChunks.ContainsKey(new Vector2(spawnX, spawnZ)))
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
        int randSpawnIndex = (int)type.x;
        int typeDiff = (int)type.y;

        if (typeDiff == 1)
        {
            go = Instantiate(EasyChunkPrefabs[randSpawnIndex]) as GameObject;
        }
        else if (typeDiff == 2)
        {
            go = Instantiate(MediumChunkPrefabs[randSpawnIndex]) as GameObject;
        }
        else if (typeDiff == 3)
        {
            go = Instantiate(HardChunkPrefabs[randSpawnIndex]) as GameObject;
        }
        else
        {
            go = Instantiate(special[randSpawnIndex]) as GameObject;
        }
        go.transform.SetParent(transform);

        go.transform.eulerAngles = new Vector3(0f, data.z, 0f);
        go.transform.position = Vector3.right * data.x + Vector3.forward * data.y;

        activeChunks.Add(new Vector2(x, z), go);
    }

    public void decreaseDiff()
    {
        if (difficulty != 1)
        {
            difficulty -= 1;
            print(difficulty);
            dm.resetTimer();
        }
        else
        {
            dm.resetTimer();
        }

    }

    public void increaseDiff()
    {
        if (difficulty != 3)
        {
            difficulty += 1;
            print(difficulty);
            dm.resetTimer();
        }
        else
        {
            dm.resetTimer();
        }
    }

}
