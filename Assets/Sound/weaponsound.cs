using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsound : MonoBehaviour
{
    public AudioSource weaponSound;

    void Start()
    {
        weaponSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponSound.Play();
        }
    }
}
