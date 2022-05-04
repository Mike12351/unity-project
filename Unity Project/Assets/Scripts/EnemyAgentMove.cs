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

    private bool flag = false;

    private const float threshold = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = patrolPoints[0].position;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        flag = true;
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
            nextPoint = new Vector3(nextPoint.x, nextPoint.y, nextPoint.z);
            if (currentIndex == index)
            {
                reached = true;
            }
            else
            {
                reached = false;
                agent.SetDestination(nextPoint);
            }
        }

        if (!agent.pathPending && !reached && flag)
        {
            if (agent.remainingDistance < threshold)
            {
                reached = true;
            }
        }
        // stay in chunk and just move around
            // if distance from enemy to player has reached a certain value (can be changed later via dynamic changes)
            //then target player
            //otherwise go back to initial pos
            //and just move around
    }
}
