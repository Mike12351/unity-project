using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float maxDistance = 10f;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //collide with only colliders in layer 7
        int layerMask = 1 << 7;

        //Will contain the information of which object the raycast hit
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * 10);

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask) && hit.collider.tag == "Interactable")
        {

            timer += Time.deltaTime;

            if (hit.collider.tag == "Interactable" && timer > 3f)
            {
                // currently chaning the tag from interactable to untagged will make sure that the gameobject slowly moves under the maze
                hit.collider.tag = "Untagged";
                // add another check that only does it if the gaze has been on object for a certain period of time
            }
        }
        else
        {
            timer = 0f;
        }

    }
}
