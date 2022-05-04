using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgentMove : MonoBehaviour
{
    //the navmesh agent
    public NavMeshAgent agent;

    //array of all the point this enemy can randomly go to
    public Transform[] patrolPoints;

    //first index is chosen randomly
    //selected index is used to check whether we chose the point that the enemy is currently on
    private int index;
    private int currentIndex = 0;

    //reached to check whether we have reached the goal
    private bool reached = true;
    //coordinates of the point to go to
    private Vector3 nextPoint;
    private Vector3 oldPoint;

    //flag to check whether the agent has been enabled
    private bool flag = false;

    private bool follow = false;

    //the constant used to determine whether we have arrived at the goal
    private const float threshold = 0.2f;
    private const float thresholdPlayer = f;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        //start by setting the enemy to the start position
        //and enable the agent component and tus the flag as well
        player = GameObject.FindWithTag("Player").transform;
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
            //choose index randomly and then compute the destination
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

        //pathpending to check whether the agent is done with calculating the path
        if (!agent.pathPending && !reached && flag)
        {
            if (agent.remainingDistance < threshold)
            {
                reached = true;
            }
        }

        // add new feature where the enemy goes after the player is the player comes close to the enemy
        // stops if the player is further away and goes back to its usual routine


        //distance between enemy and player
        float dist = Vector3.Distance(transform.position, player.position);
        //if the distance meets a certain criteria then start going towards the player
        if (dist < thresholdPlayer)
        {
            oldPoint = nextPoint;
            follow = true;
            agent.SetDestination(player.position);
        }
        //if the player is now further away then go back to initial goal
        else if (dist > thresholdPlayer && follow)
        {
            follow = false;
            agent.SetDestination(oldPoint);
        }
    }
}
