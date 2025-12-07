using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ✅ JANGAN SPAWN kalau belum pernah set SpawnPoint
        if (!PlayerPrefs.HasKey("SpawnPoint"))
            return;

        StartCoroutine(SpawnNextFrame());
    }

    IEnumerator SpawnNextFrame()
    {
        yield return null; // tunggu scene siap

        string spawnTag = PlayerPrefs.GetString("SpawnPoint");

        GameObject spawn = GameObject.FindGameObjectWithTag(spawnTag);
        if (spawn == null)
        {
            // ❌ jangan warning dulu, ini bisa scene non-planet
            yield break;
        }

        transform.SetPositionAndRotation(
            spawn.transform.position,
            spawn.transform.rotation
        );
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
