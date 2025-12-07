using UnityEngine;
using TMPro;

public class NPCInteractable : InteractableObject
{
    [Header("NPC Dialog")]
    [TextArea(3, 5)]
    public string dialogText;

    public override void Interact()
    {
        if (!isCanInteract()) return;
        base.Interact();
        ShowDialog();
        hasInteracted = true;
    }

    void ShowDialog()
    {
        // sementara pakai Debug dulu
        Debug.Log("NPC berkata: " + dialogText);
    }
}
