using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject quizPanel;     
    public AudioClip bgmPlay;

    void Start()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (settingsMenu != null) settingsMenu.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);   // Hide Quiz
    }

    public void OnStartPressed()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(false);

        if (AudioManager.I != null && bgmPlay != null)
            AudioManager.I.PlayBGM(bgmPlay);

        // Buka quiz
        if (quizPanel != null)
            quizPanel.SetActive(true);

    }

    public void OpenSettings()
    {
        if (settingsMenu != null)
            settingsMenu.SetActive(true);

        if (mainMenu != null)
            mainMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        if (mainMenu != null)
            mainMenu.SetActive(true);
    }
}
