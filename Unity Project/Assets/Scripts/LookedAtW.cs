using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookedAtW : MonoBehaviour
{
    [SerializeField]
    private Material opaque, transparent;

    private new MeshRenderer renderer;

    private bool cr_running = false;
    private bool restore = false;
    private bool is_transparent = false;

    private Coroutine lastCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if its interactable tag and if it is the case then we start the coroutine to make the wall transparent
        if (transform.tag == "Interactable")
        {
            if (!cr_running && transform.tag != "Transparent")
            {
                renderer.material = transparent;

                lastCoroutine = StartCoroutine(FadeOut());
            }
        }

        if (transform.tag == "InteractableW")
        {
            //first we either check whether it is currently running or whether it is fully transparent
            //if either is the case then we stop the last running coroutines and try to restore the wall back to normal aka not transparent
            if ((cr_running && !restore) || (!cr_running && is_transparent))
            {
                StopCoroutine(lastCoroutine);
                cr_running = false;

                if (!restore)
                {
                    lastCoroutine = StartCoroutine(FadeIn());
                    restore = true;
                }
            }
        }

    }
    IEnumerator FadeOut()
    {
        cr_running = true;
        Color c = renderer.material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            renderer.material.color = c;
            yield return new WaitForSeconds(.2f);
        }
        cr_running = false;
        transform.tag = "Transparent";
        is_transparent = true;
    }

    IEnumerator FadeIn()
    {
        cr_running = true;
        Color c = renderer.material.color;
        float current = c.a;
        for (float alpha = current; alpha <= 1; alpha += 0.1f)
        {
            c.a = alpha;
            renderer.material.color = c;
            yield return new WaitForSeconds(.2f);
        }
        cr_running = false;
        restore = false;
        renderer.material = opaque;
        transform.tag = "InteractableW";
        is_transparent = false;
    }
}
