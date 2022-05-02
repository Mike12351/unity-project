using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //max distance of the ray
    public float maxDistance = 10f;

    //amount of seconds that has passed
    private float timer;

    //amount of seconds before triggering a dynamic change
    private float threshold = 5f;

    //stores information about the last object hit by the ray
    private RaycastHit lastHit;

    //a flag to check whether a previous dynamic change is finished
    //true -> free to start the next dynamic change, false -> forcefully quit the change that is currently underway
    private bool check = true;

    //contains the manager for all potential dynamic changes
    public DynamicChangeManager dcm;

    // Update is called once per frame
    void FixedUpdate()
    {
        //collide with only colliders in layer 7
        int layerMask = 1 << 7;

        //Will contain the information of which object the raycast hit
        RaycastHit hit;

        //draws the ray, to visualize how far away the player can be
        Debug.DrawRay(transform.position, transform.forward * maxDistance);

        //if we hit an object then we can start the timer to count when we should do a dynamic change
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask))
        {
            //start the timer if we hit an object with an interactableX tag
            if (hit.collider.tag == "InteractableS" || hit.collider.tag == "InteractableW")
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }


            //if the tag of the collider ends with S then we start the hide change
            if (hit.collider.tag == "InteractableS")
            {
                //first check whether there is a previous ongoing change by checking
                //whether the lasthit collider is not empty and that the last hit collider is not the transparent change
                //at the moment there is no point reversing the hide change but if changes are required here if we plan to do so
                if (lastHit.collider != null && lastHit.collider.tag != "InteractableW")
                {
                    //if the lasthit tag is interactable meaning it is currently changing or transparent then we reverse it since we are looking at a new change right now
                    if (lastHit.collider.tag == "Interactable" || lastHit.collider.tag == "Transparent")
                    {
                        dcm.gmReset(lastHit);
                    }
                }
                //trigger the hide change if timer is bigger than the threshold
                if (timer >= threshold)
                {
                    dcm.gmHide(hit);
                    lastHit = hit;
                    timer = 0f;
                }
            }
            //if the tag ends with a W then we start the transparent change
            else if (hit.collider.tag == "InteractableW" && timer > threshold)
            {
                dcm.gmFadeIn(hit);
                if (check)
                {
                    lastHit = hit;
                    check = false;
                }
                else
                {
                    if (lastHit.collider.tag == "Interactable" || lastHit.collider.tag == "Transparent")
                    {
                        dcm.gmReset(lastHit);
                    }
                    lastHit = hit;
                    check = false;
                }
                timer = 0f;
            }
        }
        else
        {
            //now once we look away we check for the last collider that we hit and if it was a transparent wall we have to check whether
            //we fade in the bloc from the start or stop the animation and turn back
            if (lastHit.collider != null && lastHit.collider.tag != "InteractableW")
            {
                if (lastHit.collider.tag == "Interactable" || lastHit.collider.tag == "Transparent")
                {
                    dcm.gmReset(lastHit);
                    check = true;
                }
            }
            timer = 0f;
        }

    }
}
