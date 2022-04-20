using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private float timer;
    private bool newChunk = false;


    public MazeManager mm;

    private int seconds = 10;

    private int counter;

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
                print("10 seconds have passed");
                mm.decreaseDiff();
            }
        }
    }

    public void resetTimer()
    {
        print("timer reset");
        newChunk = false;
        timer = 0;
        counter = 0;
    }

    public void enteredNewChunk()
    {
        //if the player has entered a new chunk, reset the difficulty timer
        if (newChunk)
        {
            print("counter increased");
            timer = 0f;
            counter += 1;
            print(counter);
            if (counter >= 5)
            {
                counter = 0;
                mm.increaseDiff();
            }
        }else
        {
            newChunk = true;
        }
    }
}
