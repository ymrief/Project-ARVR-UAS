using UnityEngine;
using TMPro;
using System.Collections.Generic; // WAJIB ADA: Untuk Queue

public class UIPlayerManager : MonoBehaviour
{
    public static UIPlayerManager Instance;

    [Header("Interaction UI")]
    public GameObject interactionPanel;       // Panel kecil "Tekan E"
    public TextMeshProUGUI interactionText;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;          // Panel overlay hitam
    public TextMeshProUGUI dialogueText;      // Teks ucapan NPC

    // State Variable
    [HideInInspector]
    public bool isDialogueActive = false;
    private float inputCooldown = 0f;
    // Struktur Data Antrian (FIFO)
    private Queue<string> sentences;

    void Awake()
    {
        // Singleton Pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Inisialisasi Queue saat game mulai
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (inputCooldown > 0) 
        {
            inputCooldown -= Time.deltaTime;
            return; // Jangan cek input kalau masih cooldown
        }

        // Logika: Jika sedang ngobrol DAN pemain tekan E -> Lanjut kalimat berikutnya
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextSentence();
        }
    }

    // --- LOGIKA DIALOG ---

    public void StartDialogue(string[] lines)
    {
        isDialogueActive = true;
        
        inputCooldown = 0.2f;
        
        // Sembunyikan prompt interaksi agar bersih
        HideInteraction();
        
        // Tampilkan panel dialog
        dialoguePanel.SetActive(true);

        // 1. Bersihkan antrian dari dialog sebelumnya
        sentences.Clear();

        // 2. Masukkan semua kalimat baru ke dalam antrian
        foreach (string sentence in lines)
        {
            sentences.Enqueue(sentence);
        }

        // 3. Tampilkan kalimat pertama langsung
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // Cek apakah antrian sudah habis?
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Ambil satu kalimat dari antrian paling depan
        string sentence = sentences.Dequeue();
        
        // Tampilkan ke layar
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        
        // Opsional: Nyalakan kembali kursor atau gerakan player jika sebelumnya dimatikan
        Debug.Log("Dialog Selesai");
    }

    // --- LOGIKA INTERAKSI (Prompt E) ---

    public void ShowInteraction(string text)
    {
        // Jangan munculkan prompt "Tekan E" kalau sedang ngobrol
        if (isDialogueActive) return;

        interactionPanel.SetActive(true);
        interactionText.text = text;
    }

    public void HideInteraction()
    {
        interactionPanel.SetActive(false);
    }
}