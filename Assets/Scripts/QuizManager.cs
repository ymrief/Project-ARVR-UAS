using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class QA { public string question; public string[] options; public int correctIndex; }

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject quizPanel;
    public TMP_Text questionText;
    public Button[] optionButtons;
    public TMP_Text[] optionTexts;
    
    // --- TAMBAHAN BARU: Kotak untuk memasukkan ScoreUI ---
    public ScoreUI scoreUI; 
    // -----------------------------------------------------

    public List<QA> qas;

    int current = 0;
    int score = 0;

    public void ShowAt(Vector3 worldPos, Transform lookAt = null)
    {
        if (quizPanel == null) return;
        quizPanel.SetActive(true);
        quizPanel.transform.position = worldPos;
        var bb = quizPanel.GetComponentInParent<Billboard>();
        if (bb != null && lookAt != null) bb.lookAt = lookAt;
        current = 0; score = 0;
        ShowQuestion();
    }

    public void Hide()
    {
        if (quizPanel == null) return;
        quizPanel.SetActive(false);
    }

    void ShowQuestion()
    {
        if (current >= qas.Count) { EndQuiz(); return; }
        var q = qas[current];
        questionText.text = q.question;
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionTexts[i].text = (i < q.options.Length) ? q.options[i] : "";
            int idx = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnSelect(idx));
        }
    }

    void OnSelect(int idx)
    {
        if (current >= qas.Count) return;
        if (idx == qas[current].correctIndex) score++;
        current++;
        ShowQuestion();
    }

    void EndQuiz()
    {
        // --- PERBAIKAN: Nyalakan ScoreUI secara manual ---
        if (scoreUI != null) 
        {
            scoreUI.gameObject.SetActive(true); // Hidupkan objectnya
            scoreUI.ShowScore(score, qas.Count); // Tampilkan nilainya
        }
        else
        {
            Debug.LogError("⚠️ LUPA ISI: Masukkan object ScoreUI ke Inspector QuizManager!");
        }

        Hide(); // Tutup panel soal
    }
}