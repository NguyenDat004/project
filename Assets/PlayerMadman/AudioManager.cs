using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Tạo biến lưu trữ Audio Source
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    // Tạo biến lưu trữ Audio Clip
    public AudioClip musicClip;
    public AudioClip bomClip;
    public AudioClip jumpClip;

    void Start()
    {
        // Kiểm tra và phát nhạc nền nếu có
        if (musicAudioSource != null && musicClip != null)
        {
            musicAudioSource.clip = musicClip;
            musicAudioSource.loop = true; // Lặp lại nhạc nền
            musicAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Music AudioSource hoặc Music Clip chưa được gán!");
        }
    }

    // Hàm để phát âm thanh hiệu ứng (SFX) chung
    public void PlaySFX(AudioClip sfxClip)
    {
        if (vfxAudioSource != null && sfxClip != null)
        {
            vfxAudioSource.PlayOneShot(sfxClip);
        }
        else
        {
            Debug.LogWarning("VFX AudioSource hoặc SFX Clip chưa được gán!");
        }
    }

    // Phát âm thanh bom nổ
    public void PlayBomSFX()
    {
        PlaySFX(bomClip);
    }

    // Phát âm thanh nhảy
    public void PlayJumpSFX()
    {
        Debug.Log("Playing jump sound");
        vfxAudioSource.PlayOneShot(jumpClip);
    }
}
