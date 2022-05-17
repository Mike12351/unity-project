using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public DynamicChangeManager dcm;

    private float timer;
    private bool newChunk = false;

    private int seconds = 10;

    private int counter;
    private int treshold = 3;


    // variables that influence difficulty;
    //difficulty means bias towards that difficulty
    private int difficulty = 2;
    private float easy_ratio = 1f;
    private float medium_ratio = 1f;
    private float hard_ratio = 1f;
    private float increaseRatioCoeff = 0.25f;

    private const float maxRatioCoeff = 3f;
    private const float minRatioCoeff = 0.5f;

    private const float range = 100;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        counter = 0;

        //due to start difficulty being set to 2
        medium_ratio = 2f;
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
                decreaseDiff();
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
            if (counter >= treshold)
            {
                counter = 0;
                increaseDiff();
            }
        }else
        {
            newChunk = true;
        }
    }

    public int getDiff()
    {
        int index = Random.Range(0, Mathf.FloorToInt((range * (easy_ratio + medium_ratio + hard_ratio))));
        int result = 1;
        if (index < easy_ratio*range)
        {
            result = 1;
        }
        else if (index < range + medium_ratio*range)
        {
            result = 2;
        }
        else
        {
            result = 3;
        }
        print("easy ratio = " + easy_ratio);
        print("med ratio = " + medium_ratio);
        print("hard ratio = " + hard_ratio);
        return result;
    }

    public void increaseRatio(int diff)
    {
        if (diff == 1)
        {
            if (easy_ratio < maxRatioCoeff)
            {
                easy_ratio = easy_ratio + increaseRatioCoeff;
            }
        }
        else if (diff == 2)
        {
            if (medium_ratio < maxRatioCoeff)
            {
                medium_ratio = medium_ratio + increaseRatioCoeff;
            }
        }
        else
        {
            if (hard_ratio < maxRatioCoeff)
            {
                hard_ratio = hard_ratio + increaseRatioCoeff;
            }
        }
    }

    public void decreaseRatio(int diff)
    {
        if (diff == 1)
        {
            if (easy_ratio > minRatioCoeff)
            {
                easy_ratio = easy_ratio - increaseRatioCoeff;
            }
        }
        else if (diff == 2)
        {
            if (medium_ratio > minRatioCoeff)
            {
                medium_ratio = medium_ratio - increaseRatioCoeff;
            }
        }
        else
        {
            if (hard_ratio > minRatioCoeff)
            {
                hard_ratio = hard_ratio - increaseRatioCoeff;
            }
        }
    }

    public void increaseDiff()
    {
        if (difficulty != 3)
        {
            difficulty += 1;
            resetTimer();
            //reset ratio
            resetRatio(difficulty);
        }
        else
        {
            increaseRatio(difficulty);
            print("incr");
            resetTimer();
        }
    }

    public void decreaseDiff()
    {
        if (difficulty != 1)
        {
            difficulty -= 1;
            resetTimer();
            resetRatio(difficulty);
        }
        else
        {
            decreaseRatio(difficulty);
            print("decr");
            resetTimer();
        }

    }

    public void resetRatio(int diff)
    {
        easy_ratio = 1f;
        medium_ratio = 1f;
        hard_ratio = 1f;

        if (diff == 1)
        {
            easy_ratio = 2f;
        }else if (diff == 2)
        {
            medium_ratio = 2f;
        }
        else
        {
            hard_ratio = 2f;
        }
    }
}
