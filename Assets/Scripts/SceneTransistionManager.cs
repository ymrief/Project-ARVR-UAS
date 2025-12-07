using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Fade")]
    public CanvasGroup fadeCanvas;       // assign: CanvasGroup pada FadeCanvas (UIRoot)
    public float fadeDuration = 0.5f;

    [Header("Loading")]
    public bool useLoadingScene = false; // kalau mau pakai LoadingScene terpisah (opsional)
    public string loadingSceneName = "Loading";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeCanvas != null)
        {
            // ensure initial state: invisible and not blocking raycasts
            fadeCanvas.alpha = 0f;
            fadeCanvas.blocksRaycasts = false;
            fadeCanvas.interactable = false;
        }
    }

    /// <summary>
    /// Public API: panggil dari Portal
    /// </summary>
    public void LoadScene(string sceneName, string spawnTag)
    {
        // simpan spawn tag untuk PlayerManager
        PlayerPrefs.SetString("SpawnPoint", spawnTag);

        StartCoroutine(TransitionCoroutine(sceneName));
    }

    IEnumerator TransitionCoroutine(string sceneName)
    {
        // 1) Fade out (block input)
        yield return FadeTo(1f);

        // 2) Optional: go to a minimal "LoadingScene" first (UI-only)
        if (useLoadingScene && SceneManager.GetActiveScene().name != loadingSceneName)
        {
            // load loading scene synchronously (fast) so player sees loading UI immediately
            SceneManager.LoadScene(loadingSceneName);
            // wait one frame to ensure loading scene displayed
            yield return null;
        }

        // 3) Async load target scene but prevent immediate activation until ready
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // wait until almost done (0.9) -> textures/shaders mostly prepared
        while (op.progress < 0.9f)
            yield return null;

        // give one frame for GPU/objects to finalize
        yield return new WaitForEndOfFrame();

        // 4) Activate scene
        op.allowSceneActivation = true;

        // wait until scene actually activated
        while (!op.isDone)
            yield return null;

        // 5) Start fade-in IMMEDIATELY (parallel dengan finishing touches)
        Coroutine fadeInCoroutine = StartCoroutine(FadeTo(0f));

        // 6) Wait one frame so Awake/Start of scene objects run
        yield return null;

        // 7) Wait for fade-in to complete
        yield return fadeInCoroutine;
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        if (fadeCanvas == null)
            yield break;

        float start = fadeCanvas.alpha;
        float elapsed = 0f;

        // when fading out to 1 => block input
        if (targetAlpha > 0f)
        {
            fadeCanvas.blocksRaycasts = true;
            fadeCanvas.interactable = true;
        }

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // use unscaled so UI fades even if timeScale changed
            fadeCanvas.alpha = Mathf.Lerp(start, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = targetAlpha;

        // when fading in to 0 => unblock input
        if (targetAlpha == 0f)
        {
            fadeCanvas.blocksRaycasts = false;
            fadeCanvas.interactable = false;
        }
    }
}
