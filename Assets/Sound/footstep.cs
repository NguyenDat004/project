using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstep : MonoBehaviour
{
    public AudioSource footstepsound;
    // Start is called before the first frame update
    void Start()
    {
        footstepsound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            footstepsound.Play();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            footstepsound.Play();
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            footstepsound.Stop();
        }
    }
}
