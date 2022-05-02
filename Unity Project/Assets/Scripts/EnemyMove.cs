using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    public Transform[] patrolPoints;
    private int index;
    public EnemyManager ep;

    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ep = GameObject.FindObjectOfType<EnemyManager>();
        index = 0;
        transform.position = patrolPoints[index].position;
        moveSpeed = ep.getSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = ep.getSpeed();

        if (transform.position == patrolPoints[index].position)
        {
            index++;
        }

        if (index >= patrolPoints.Length)
        {
            index = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[index].position, moveSpeed * Time.deltaTime);

    }
}
