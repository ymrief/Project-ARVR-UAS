using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [Header("Basic Info")]
    public string interactionName = "Nama Objek";

    [Header("Interaction")]
    public bool oneTimeOnly = false;

    protected bool hasInteracted = false;

    // Method ini dipanggil PlayerInteraction
    public virtual void Interact()
    {
        if (!isCanInteract()) return;

        hasInteracted = true;

        Debug.Log("Interaksi dengan: " + interactionName);
    }

    public bool isCanInteract()
    {
        return !oneTimeOnly || !hasInteracted;
    }
}
