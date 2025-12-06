using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI scoreText;
    public FadeController fadeController;

    void Start() { if (panel) panel.SetActive(false); }

    public void ShowScore(int score, int total)
    {
        if (panel) panel.SetActive(true);
        if (scoreText != null) scoreText.text = $"Skor: {score}/{total}";
    }

    public void OnFinishPressed()
    {
        if (fadeController != null) fadeController.FadeToEnd();
    }
}
