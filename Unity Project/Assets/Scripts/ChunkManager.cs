using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject[] chunkPrefabs;

    private Transform playerTransform;
    private float chunkLength = 20f;
    private int arrayLength;

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
        
    }

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

    }

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
}
