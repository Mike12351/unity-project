using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float moveSpeed = 6f;
    private float baseSpeed = 6f;
    private float sprintSpeed = 18f;

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
