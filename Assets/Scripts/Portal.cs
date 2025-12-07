using UnityEngine;

public class Portal : InteractableObject
{
    [Header("Target")]
    public string targetScene;
    public string spawnPointTag;

    public override void Interact()
    {
        // Safety check
        if (SceneTransitionManager.Instance == null)
        {
            Debug.LogError("SceneTransitionManager belum ada di scene!");
            return;
        }

        // Simpan spawn point (dipakai PlayerManager)
        PlayerPrefs.SetString("SpawnPoint", spawnPointTag);

        // Load scene via transition system (fade + async)
        SceneTransitionManager.Instance.LoadScene(
            targetScene,
            spawnPointTag
        );
    }
}
