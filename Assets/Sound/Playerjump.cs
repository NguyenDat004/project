using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public AudioSource jumpSound;

    void Start()
    {
        jumpSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Giả sử phím Space là phím nhảy
        {
            jumpSound.Play();
        }
    }
}
