using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    public Transform[] patrolPoints;
    public float moveSpeed;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        transform.position = patrolPoints[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        
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
