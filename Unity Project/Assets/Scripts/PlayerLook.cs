using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float maxDistance = 10f;

    private float timer;

    private RaycastHit lastHit;

    private bool check = true;

    private float threshold = 5f;

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

        //if we hit an object then we can start the timer to count when we should do a dynamic change
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask))
        {
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
                if (lastHit.collider != null && lastHit.collider.tag != "InteractableW")
                {
                    if (lastHit.collider.tag == "Interactable" || lastHit.collider.tag == "Transparent")
                    {
                        lastHit.collider.tag = "InteractableW";
                    }
                }
                if (timer >= threshold)
                {
                    hit.collider.tag = "Hide";
                    lastHit = hit;
                    timer = 0f;
                }
            }
            //if the tag ends with a W then we start the transparent change
            else if (hit.collider.tag == "InteractableW" && timer > threshold)
            {
                hit.collider.tag = "Interactable";
                if (check)
                {
                    lastHit = hit;
                    check = false;
                }
                else
                {
                    if (lastHit.collider.tag == "Interactable" || lastHit.collider.tag == "Transparent")
                    {
                        lastHit.collider.tag = "InteractableW";
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
                    lastHit.collider.tag = "InteractableW";
                    check = true;
                }
            }
            timer = 0f;
        }

    }
}
