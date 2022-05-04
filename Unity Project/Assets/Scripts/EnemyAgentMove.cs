using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgentMove : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform[] patrolPoints;

    private int index;
    private int currentIndex = 0;

    private bool reached = true;
    private Vector3 nextPoint;

    private const float threshold = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = patrolPoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        //if the enemy hasnt reached the current goal point do nothing
        //otherwise repeat and set a new goal point
        if (reached)
        {
            index = Random.Range(0, patrolPoints.Length);
            nextPoint = patrolPoints[index].position;
            reached = false;
            nextPoint = new Vector3(nextPoint.x, nextPoint.y, nextPoint.z);
            //if (currentIndex == index)
            agent.SetDestination(nextPoint);
        }
        // stay in chunk and just move around

        if (agent.remainingDistance < threshold && !reached)
        {
            //if the remaining distance is less than the threshold then we assume that the player has arrived
            //at the destination
            reached = true;
            print("reached destination");
        }
            // if distance from enemy to player has reached a certain value (can be changed later via dynamic changes)
            //then target player
            //otherwise go back to initial pos
            //and just move around
    }
}
