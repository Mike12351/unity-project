using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookedAt : MonoBehaviour
{
    private Vector3 endPos;

    [SerializeField]
    private Material glow;

    // Start is called before the first frame update
    void Start()
    {
        endPos = new Vector3(transform.position.x, -3.99f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.tag == "Untagged")
        {
            MeshRenderer renderer =  GetComponent<MeshRenderer>();

            renderer.material = glow;

            if (transform.position != endPos)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 1f);

            }
        }
    }
}
