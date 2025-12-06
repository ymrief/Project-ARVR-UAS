using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image blackImage;
    public TextMeshProUGUI thanksText;
    public float fadeDur = 1.2f;

    void Start()
    {
        SetAlpha(0f);
        if (thanksText != null) thanksText.alpha = 0f;
    }

    public void FadeToEnd()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        float t = 0f;
        while (t < fadeDur)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(t / fadeDur));
            yield return null;
        }

        float tt = 0f;
        float showDur = 1.5f;
        while (tt < showDur)
        {
            tt += Time.deltaTime;
            if (thanksText != null) thanksText.alpha = Mathf.Clamp01(tt / showDur);
            yield return null;
        }
    }

    void SetAlpha(float a)
    {
        if (blackImage == null) return;
        var c = blackImage.color;
        c.a = a;
        blackImage.color = c;
    }
}
