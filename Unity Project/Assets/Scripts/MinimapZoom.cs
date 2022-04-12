using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapZoom : MonoBehaviour
{

    private float currentZoom;
    public new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        currentZoom = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentZoom == 10)
            {
                currentZoom = 20;
            }
            else
            {
                currentZoom = 10;
            }
            camera.orthographicSize = currentZoom;
        }
    }
}
