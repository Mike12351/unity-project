using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{

    /*
     This script is attached to objects that will be destroyed after a certain time has passed
     the variable time is used to set how long before the object is destroyed
     */
    public float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }
}
