using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    // Start is called before the first frame update
    private bool change = false;

    [SerializeField]
    private Material wall;
    private new MeshRenderer renderer;
    private Vector3 endPos;

    void Start()
    {
        endPos = new Vector3(transform.position.x, 1f, transform.position.z);
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if the tag of the gameoject is hide and if it is the case then we start the method to hide the bloc underneath the map
        if (change)
        {
            if (transform.position != endPos)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 1f);
            }
            else
            {
                Destroy(this);
            }
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        renderer.material = wall;
        change = true;
    }

}
