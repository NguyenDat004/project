using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Tạo biến lưu trữ Audio source

    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    //Tạo biến lưu trữ Audio Clip

    public AudioClip musicClip;
    public AudioClip bomClip;
    public AudioClip jumpClip;
    void Start()
    { 
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
        
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        vfxAudioSource.clip = sfxClip;
        vfxAudioSource.PlayOneShot(sfxClip);
        
    }

    // Phát âm thanh nhảy
    public void PlayJumpSFX()
    {
        vfxAudioSource.PlayOneShot(jumpClip);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
