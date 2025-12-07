using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI scoreText;
    public FadeController fadeController;

    // --- KITA HAPUS FUNGSI START() AGAR TIDAK MATI SENDIRI ---
    
    // Fungsi ini dipanggil oleh QuizManager
    public void ShowScore(int score, int total)
    {
        // 1. Pastikan Panel Nyala
        if (panel != null) 
        {
            panel.SetActive(true);
        }
        
        // 2. Tulis Skornya
        if (scoreText != null) 
        {
            scoreText.text = $"Skor: {score}/{total}";
            Debug.Log($"[ScoreUI] Menampilkan Skor Akhir: {score}/{total}");
        }
    }

    public void OnFinishPressed()
    {
        if (fadeController != null) fadeController.FadeToEnd();
    }
}