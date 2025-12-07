using System.Collections;
using System.Collections.Generic;
using System.Linq; // Wajib untuk fitur pengocok (Shuffle)
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script Utama Role 4: AI & Quiz System.
/// Fitur: Request Soal ke Gemini -> Parsing Data -> Gameplay Manual (Keyboard).
/// </summary>
public class GeminiManager : MonoBehaviour
{
    [Header("--- 1. PENGATURAN API ---")]
    [Tooltip("Masukkan API Key 'Default Gemini Project' (...6YQA)")]
    public string apiKey = "MASUKKAN_KEY_DEFAULT_ANDA_DISINI";
    
    // URL Model Gemini 2.5 Flash (Tercepat & Stabil)
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

    [Header("--- 2. DATA GAME ---")]
    public List<QuizData> daftarSoal = new List<QuizData>(); 
    public int score = 0;
    
    // Variabel Internal untuk Logika Game
    private int nomorSoalAktif = 0; 
    private bool isGameReady = false;

    void Start()
    {
        Debug.Log("--- [SYSTEM] MEMULAI KONEKSI KE GEMINI... ---");
        GenerateContent();
    }

    // ==================================================================================
    // BAGIAN A: INPUT CONTROLLER (Keyboard Laptop)
    // ==================================================================================
    void Update()
    {
        // Jangan baca keyboard kalau soal belum siap atau game sudah tamat
        if (!isGameReady) return;

        // Deteksi Tombol A, B, atau C
        if (Input.GetKeyDown(KeyCode.A)) JawabManual("A");
        if (Input.GetKeyDown(KeyCode.B)) JawabManual("B");
        if (Input.GetKeyDown(KeyCode.C)) JawabManual("C");
    }

    void JawabManual(string jawaban)
    {
        // 1. Cek Jawaban
        CheckAnswer(nomorSoalAktif, jawaban);

        // 2. Pindah ke nomor berikutnya
        nomorSoalAktif++;

        // 3. Cek kondisi game (Lanjut atau Tamat?)
        if (nomorSoalAktif < daftarSoal.Count)
        {
            TampilkanSoalDiConsole(nomorSoalAktif);
        }
        else
        {
            SelesaikanGame();
        }
    }

    // ==================================================================================
    // BAGIAN B: GENERATE CONTENT (Koneksi ke Google)
    // ==================================================================================
    public void GenerateContent()
    {
        // 1. Siapkan & Acak Planet
        List<string> semuaPlanet = new List<string> { 
            "Merkurius", "Venus", "Bumi", "Mars", 
            "Jupiter", "Saturnus", "Uranus", "Neptunus" 
        };
        semuaPlanet = semuaPlanet.OrderBy(x => Random.value).ToList();
        
        // 2. Buat Prompt Dinamis (3 Planet Berbeda)
        string prompt = $"Buatkan 3 soal kuis pilihan ganda SANGAT MUDAH (level SD) dengan ketentuan:\n" +
                        $"- Soal 1 membahas planet {semuaPlanet[0]}\n" +
                        $"- Soal 2 membahas planet {semuaPlanet[1]}\n" +
                        $"- Soal 3 membahas planet {semuaPlanet[2]}\n" +
                        "Gunakan bahasa Indonesia sederhana. Setiap soal punya opsi A, B, C.\n" +
                        "PENTING: Tulis output DENGAN FORMAT RAW berikut ini tanpa teks lain:\n\n" +
                        "SOAL#Opsi A#Opsi B#Opsi C#KunciJawaban\n" +
                        "SOAL#Opsi A#Opsi B#Opsi C#KunciJawaban\n" +
                        "SOAL#Opsi A#Opsi B#Opsi C#KunciJawaban";

        StartCoroutine(CallApi(prompt));
    }

    IEnumerator CallApi(string prompt)
    {
        // Bungkus JSON (Anti Error 400)
        RequestBody reqBody = new RequestBody();
        reqBody.contents = new RequestContent[] { new RequestContent { parts = new RequestPart[] { new RequestPart { text = prompt } } } };
        
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(reqBody));
        string fullUrl = apiUrl + "?key=" + apiKey;

        // Kirim Request
        UnityWebRequest request = new UnityWebRequest(fullUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(request.downloadHandler.text);
            if (response.candidates != null && response.candidates.Length > 0)
            {
                ProcessRawTextToQuiz(response.candidates[0].content.parts[0].text);
            }
        }
        else
        {
            Debug.LogError($"[ERROR API] Pesan: {request.error}");
        }
    }

    void ProcessRawTextToQuiz(string text)
    {
        daftarSoal.Clear();
        score = 0;
        nomorSoalAktif = 0;

        // Parsing Teks menjadi Data Game
        string[] lines = text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] parts = line.Split('#');
            if (parts.Length >= 5) 
            {
                QuizData soalBaru = new QuizData();
                soalBaru.pertanyaan = parts[0].Trim();
                soalBaru.opsiA = parts[1].Trim();
                soalBaru.opsiB = parts[2].Trim();
                soalBaru.opsiC = parts[3].Trim();
                soalBaru.kunciJawaban = parts[4].Trim().ToUpper();
                daftarSoal.Add(soalBaru);
            }
        }

        Debug.Log($"<color=cyan>[SYSTEM] {daftarSoal.Count} Soal Siap! Silakan Main.</color>");
        
        // Mulai Game
        isGameReady = true;
        TampilkanSoalDiConsole(0);
    }

    // ==================================================================================
    // BAGIAN C: LOGIKA KUIS (Check Answer & Display)
    // ==================================================================================
    public void CheckAnswer(int nomorSoal, string jawabanPlayer)
    {
        if (nomorSoal >= daftarSoal.Count) return;

        QuizData soal = daftarSoal[nomorSoal];
        Debug.Log($"[Input] Anda Jawab: {jawabanPlayer}");

        if (jawabanPlayer == soal.kunciJawaban)
        {
            score += 10;
            Debug.Log("<color=green>JAWABAN BENAR (+10 Poin)</color>");
        }
        else
        {
            Debug.Log($"<color=red>JAWABAN SALAH (Kunci: {soal.kunciJawaban})</color>");
        }
    }

    void TampilkanSoalDiConsole(int index)
    {
        QuizData q = daftarSoal[index];
        Debug.Log("--------------------------------------------------");
        Debug.Log($"<b>SOAL {index + 1}:</b> {q.pertanyaan}");
        Debug.Log($"A. {q.opsiA}");
        Debug.Log($"B. {q.opsiB}");
        Debug.Log($"C. {q.opsiC}");
        Debug.Log("<color=yellow>>> TEKAN TOMBOL A, B, atau C DI KEYBOARD UNTUK MENJAWAB <<</color>");
    }

    void SelesaikanGame()
    {
        Debug.Log("=================================");
        Debug.Log($"<b>GAME SELESAI!</b>");
        Debug.Log($"<b>SKOR AKHIR ANDA: {score} / 30</b>");
        Debug.Log("=================================");
        isGameReady = false; // Matikan input
    }

    // ==================================================================================
    // BAGIAN D: STRUKTUR DATA (JSON CLASSES)
    // ==================================================================================
    [System.Serializable] public class QuizData { public string pertanyaan, opsiA, opsiB, opsiC, kunciJawaban; }
    [System.Serializable] public class RequestBody { public RequestContent[] contents; }
    [System.Serializable] public class RequestContent { public RequestPart[] parts; }
    [System.Serializable] public class RequestPart { public string text; }
    [System.Serializable] public class GeminiResponse { public Candidate[] candidates; }
    [System.Serializable] public class Candidate { public Content content; }
    [System.Serializable] public class Content { public Part[] parts; }
    [System.Serializable] public class Part { public string text; }
}