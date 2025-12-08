using UnityEngine;
using TMPro;
using UnityEngine.UI; // Wajib ada

public class ScoreUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;            
    public TextMeshProUGUI scoreText;   
    public Button tombolUlang;          
    
    // --- SLOT BARU UNTUK TOMBOL KELUAR ---
    public Button tombolKeluar;         

    void Start()
    {
        // 1. Setup Tombol Ulang
        if (tombolUlang != null)
        {
            tombolUlang.onClick.RemoveAllListeners();
            tombolUlang.onClick.AddListener(ProsesUlang);
        }

        // 2. Setup Tombol Keluar (BARU)
        if (tombolKeluar != null)
        {
            tombolKeluar.onClick.RemoveAllListeners();
            tombolKeluar.onClick.AddListener(ProsesKeluar);
        }
    }

    public void ShowScore(int score, int total)
    {
        if (panel != null) panel.SetActive(true);
        
        if (scoreText != null) 
        {
            scoreText.text = $"SCORE AKHIR : {score} / {total}";
        }
    }

    void ProsesUlang()
    {
        // Logic Main Lagi (sama seperti sebelumnya)
        if (panel != null) panel.SetActive(false);
        GeminiManager gemini = FindObjectOfType<GeminiManager>();
        if (gemini != null) gemini.GenerateContent();
    }

    // --- FUNGSI BARU: KELUAR GAME ---
    void ProsesKeluar()
    {
        Debug.Log("[Game] Tombol Keluar ditekan. Bye bye!");
        
        // Perintah untuk menutup aplikasi (saat sudah jadi file .exe / .apk)
        Application.Quit();

        // Kode tambahan supaya tombolnya bekerja juga di Editor Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}