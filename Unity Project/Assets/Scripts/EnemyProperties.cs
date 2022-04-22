using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public float moveSpeed = 6f;
    private float baseSpeed = 6f;
    private float sprintSpeed = 18f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
