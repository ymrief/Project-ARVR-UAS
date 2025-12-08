using UnityEngine;

public class PressETrigger : MonoBehaviour
{
    public GameObject pressEUI;
    public GameObject dialogPanel;
    public string npcName = "Guide";
    public string dialogText = "Selamat datang! Tekan Start untuk mulai.";

    bool playerInside = false;

    void Start()
    {
        if (pressEUI != null) pressEUI.SetActive(false);
        if (dialogPanel != null) dialogPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // --- TAMBAHAN UNTUK CEK ---
        Debug.Log("Sesuatu menabrak NPC! Bernama: " + other.name + " | Tag: " + other.tag);
        // ---------------------------

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("HORE! Player terdeteksi!"); // <-- Cek ini juga
            if (pressEUI != null) pressEUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (pressEUI != null) pressEUI.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerInside) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pressEUI != null) pressEUI.SetActive(false);
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(true);
                var ui = dialogPanel.GetComponent<NPCDialogUI>();
                if (ui != null) ui.SetDialog(npcName, dialogText);
                if (ui != null) ui.Show();
            }
        }
    }
}
