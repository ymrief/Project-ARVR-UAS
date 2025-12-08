using UnityEngine;

public class NPCInteractable : InteractableObject
{
    [Header("NPC Dialog Data")]
    [TextArea(3, 10)] // Membuat kotak input di Inspector lebih besar
    public string[] dialogueLines; // Array String (Bisa isi banyak kalimat)

    public override void Interact()
    {
        // Cek kondisi dasar (jarak/halangan) dari script induk
        if (!isCanInteract()) return;
        
        base.Interact();

        // Kirim data array ke UI Manager untuk diproses
        if (UIPlayerManager.Instance != null)
        {
            UIPlayerManager.Instance.StartDialogue(dialogueLines);
        }
        
        hasInteracted = true;
    }
}