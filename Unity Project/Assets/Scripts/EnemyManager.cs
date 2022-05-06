using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    private float baseSpeed = 3.5f;
    private float sprintSpeed = 5f;

    public void sprint()
    {
        moveSpeed = sprintSpeed;
    }

    public void resetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    public float getSpeed()
    {
        return moveSpeed;
    }

    public void freeze()
    {
        moveSpeed = 0;
    }
}
