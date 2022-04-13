using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private float timer;
    private bool newChunk = false;


    public ChunkManager cm;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player enters a new chunk
        //start the timer and see how long the player stays in that chunk
        if (newChunk)
        {
            timer += Time.deltaTime;

            if (timer > 3)
            {
                print("3 seconds have passed");
                cm.decreaseDiff();
            }
        }
    }

    public void resetTimer()
    {
        newChunk = false;
        timer = 0;
    }

    public void setNewChunk()
    {
        if (newChunk)
        {
            timer = 0f;
            print("timer reset");
        }else
        {
            newChunk = true;
        }
    }
}
