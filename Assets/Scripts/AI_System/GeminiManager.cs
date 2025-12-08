using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// GeminiManager V11.0 (Final Fix)
/// Fitur: Request ke Google -> Bersihkan Kunci Jawaban (Anti Error Nilai 0) -> Kirim ke UI.
/// </summary>
public class GeminiManager : MonoBehaviour
{
    [Header("--- 1. KONEKSI KE UI (WAJIB DIISI) ---")]
    [Tooltip("Tarik object 'Quiz_UI' dari Hierarchy ke sini!")]
    public QuizManager quizUIRef; 

    [Header("--- 2. PENGATURAN API ---")]
    [Tooltip("Masukkan API Key di Inspector (Jangan di script agar aman)")]
    public string apiKey = ""; 
    
    // URL Model Gemini 2.5 Flash
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

    // Data mentah untuk debug
    private List<QuizData> rawData = new List<QuizData>(); 

    void Start()
    {
        // Cek apakah referensi UI sudah masuk
        if (quizUIRef == null)
        {
            Debug.LogError("⛔ ERROR: Kolom 'Quiz UI Ref' di Inspector GeminiManager masih KOSONG! Tarik object Quiz_UI kesini.");
            return;
        }

        Debug.Log("[Gemini] Memulai proses request soal...");
        GenerateContent();
    }

    // --- BAGIAN A: GENERATE CONTENT ---
    public void GenerateContent()
    {
        // 1. Acak Planet
        List<string> planets = new List<string> { 
            "Merkurius", "Venus", "Bumi", "Mars", "Jupiter", "Saturnus", "Uranus", "Neptunus" 
        };
        planets = planets.OrderBy(x => Random.value).ToList();
        
        // 2. Buat Prompt
        string prompt = $"Buatkan 3 soal kuis pilihan ganda SANGAT MUDAH (level SD) dengan ketentuan:\n" +
                        $"- Soal 1: {planets[0]}\n- Soal 2: {planets[1]}\n- Soal 3: {planets[2]}\n" +
                        "Format RAW Wajib (Pemisah pakai tanda pagar #):\n\n" +
                        "SOAL#Opsi A#Opsi B#Opsi C#KunciJawaban\n" +
                        "SOAL#Opsi A#Opsi B#Opsi C#KunciJawaban\n" +
                        "SOAL#Opsi A#Opsi B#Opsi C#KunciJawaban";

        StartCoroutine(CallApi(prompt));
    }

    IEnumerator CallApi(string prompt)
    {
        RequestBody reqBody = new RequestBody();
        reqBody.contents = new RequestContent[] { new RequestContent { parts = new RequestPart[] { new RequestPart { text = prompt } } } };
        
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(reqBody));
        string fullUrl = apiUrl + "?key=" + apiKey;

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
                ProcessAndSendToUI(response.candidates[0].content.parts[0].text);
            }
        }
        else
        {
            Debug.LogError($"[Gemini Error] {request.error}");
        }
    }

    // --- BAGIAN B: PROSES DATA & KIRIM KE UI (LOGIC DIPERBAIKI) ---
    void ProcessAndSendToUI(string text)
    {
        rawData.Clear();
        List<QA> uiQuestions = new List<QA>();

        string[] lines = text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string line in lines)
        {
            string[] parts = line.Split('#');
            if (parts.Length >= 5) 
            {
                // 1. Ambil Data Mentah
                QuizData qData = new QuizData();
                qData.pertanyaan = parts[0].Trim();
                qData.opsiA = parts[1].Trim();
                qData.opsiB = parts[2].Trim();
                qData.opsiC = parts[3].Trim();
                
                // --- LOGIC BARU: PEMBERSIH KUNCI JAWABAN ---
                // Mengambil kunci mentah, misal: "A." atau "Jawaban: B" atau " C "
                string rawKey = parts[4].Trim().ToUpper();
                
                int finalIndex = 0; // Default A
                
                // Cek kandungan hurufnya saja (lebih aman)
                if (rawKey.Contains("A")) finalIndex = 0;
                else if (rawKey.Contains("B")) finalIndex = 1;
                else if (rawKey.Contains("C")) finalIndex = 2;
                
                // Simpan kunci yang sudah bersih
                qData.kunciJawaban = (finalIndex == 0 ? "A" : (finalIndex == 1 ? "B" : "C"));
                rawData.Add(qData);

                // 2. Bungkus ke Format UI (QA)
                QA newQA = new QA();
                newQA.question = qData.pertanyaan;
                newQA.options = new string[] { qData.opsiA, qData.opsiB, qData.opsiC };
                newQA.correctIndex = finalIndex; // Masukkan index angka yang benar (0/1/2)

                uiQuestions.Add(newQA);
                
                // Debugging di Console untuk memastikan kunci benar
                Debug.Log($"Soal: {qData.pertanyaan.Substring(0, 10)}... | Kunci Terbaca: {qData.kunciJawaban} (Index: {finalIndex})");
            }
        }

        Debug.Log($"<color=cyan>[Gemini] Berhasil memproses {uiQuestions.Count} soal. Mengirim ke QuizManager...</color>");

        // 3. Kirim ke QuizManager
        if (quizUIRef != null)
        {
            quizUIRef.qas = uiQuestions;
            // Tampilkan UI di posisi aslinya (agar tidak error kamera)
            quizUIRef.ShowAt(quizUIRef.transform.position); 
        }
    }

    // --- DATA CLASSES ---
    [System.Serializable] public class QuizData { public string pertanyaan, opsiA, opsiB, opsiC, kunciJawaban; }
    [System.Serializable] public class RequestBody { public RequestContent[] contents; }
    [System.Serializable] public class RequestContent { public RequestPart[] parts; }
    [System.Serializable] public class RequestPart { public string text; }
    [System.Serializable] public class GeminiResponse { public Candidate[] candidates; }
    [System.Serializable] public class Candidate { public Content content; }
    [System.Serializable] public class Content { public Part[] parts; }
    [System.Serializable] public class Part { public string text; }
}