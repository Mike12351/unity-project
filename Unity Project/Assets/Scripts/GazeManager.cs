using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reset(RaycastHit hit)
    {
        hit.collider.tag = "InteractableW";
    }

    public void hide(RaycastHit hit)
    {
        hit.collider.tag = "Hide";
    }

    public void fadeIn(RaycastHit hit)
    {
        hit.collider.tag = "Interactable";
    }
}
