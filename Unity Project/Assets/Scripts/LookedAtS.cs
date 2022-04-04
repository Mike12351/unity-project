using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookedAtS : MonoBehaviour
{
    private Vector3 endPos;

    [SerializeField]
    private Material wall;

    private new MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        endPos = new Vector3(transform.position.x, -3.99f, transform.position.z);
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if the tag of the gameoject is hide and if it is the case then we start the method to hide the bloc underneath the map
        if (transform.tag == "Hide")
        {
            renderer.material = wall;

            if (transform.position != endPos)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 1f);
            }
        }
    }
}
