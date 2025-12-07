using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    string targetScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator Start()
    {
        // hanya jalan di LoadingScene
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            yield return null; // tunggu 1 frame

            AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                yield return null;
            }

            // FRAME TAMBAHAN BIAR TEXTURE READY
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.2f);

            op.allowSceneActivation = true;
        }
    }
}
