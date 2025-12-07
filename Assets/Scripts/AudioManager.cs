using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (I == null) { I = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SetBGMVolume(float v) { bgmSource.volume = v; }
    public void SetSFXVolume(float v) { sfxSource.volume = v; }
}
