using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager_Update : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;   
    public AudioClip bgmPlay;
    public string targetScene = "Home";
    public string spawnPointTag = "SpawnPoint";
    void Start()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (settingsMenu != null) settingsMenu.SetActive(false);
    }

    public void OnStartPressed()
    {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(false);

        SceneTransitionManager.Instance.LoadScene(
            targetScene,
            spawnPointTag
        );
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
