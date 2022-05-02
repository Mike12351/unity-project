using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private float timer;
    private bool newChunk = false;

    public DynamicChangeManager dcm;

    private int seconds = 30;

    private int counter;
    private int treshold = 10;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player enters a new chunk
        //start the timer and see how long the player stays in that chunk
        if (newChunk)
        {
            timer += Time.deltaTime;

            if (timer > seconds)
            {
                print("30 seconds have passed");
                dcm.mmDecreaseDiff();
            }
        }
    }

    public void resetTimer()
    {
        //print("timer reset");
        newChunk = false;
        timer = 0;
        counter = 0;
    }

    public void enteredNewChunk()
    {
        //if the player has entered a new chunk, reset the difficulty timer
        if (newChunk)
        {
            //print("counter increased");
            timer = 0f;
            counter += 1;
            //print(counter);
            if (counter >= treshold)
            {
                counter = 0;
                dcm.mmIncreaseDiff();
            }
        }else
        {
            newChunk = true;
        }
    }
}
