using UnityEngine;
using TMPro;

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [Header("Interaction UI")]
    public TextMeshProUGUI interactionText;

    // ✅ cache state
    bool isVisible = false;
    string lastText = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ✅ PREFETCH UI (very important)
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true); // aktifkan sekali
            interactionText.text = "";                  // kosongkan
            interactionText.alpha = 0f;                 // sembunyikan via alpha
        }
    }

    public void ShowInteraction(string text)
    {
        if (interactionText == null) return;

        // ✅ jangan update kalau teks sama & sudah tampil
        if (isVisible && lastText == text) return;

        lastText = text;
        isVisible = true;

        interactionText.text = text;
        interactionText.alpha = 1f;   // ✅ NO SetActive
    }

    public void HideInteraction()
    {
        if (interactionText == null) return;
        if (!isVisible) return;

        isVisible = false;
        lastText = "";

        interactionText.alpha = 0f;   // ✅ NO SetActive
    }
}
