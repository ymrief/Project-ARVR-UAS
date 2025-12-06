using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (AudioManager.I != null)
        {
            if (bgmSlider != null) bgmSlider.value = AudioManager.I.bgmSource.volume;
            if (sfxSlider != null) sfxSlider.value = AudioManager.I.sfxSource.volume;

            if (bgmSlider != null) bgmSlider.onValueChanged.AddListener((v) => AudioManager.I.SetBGMVolume(v));
            if (sfxSlider != null) sfxSlider.onValueChanged.AddListener((v) => AudioManager.I.SetSFXVolume(v));
        }
    }
}

