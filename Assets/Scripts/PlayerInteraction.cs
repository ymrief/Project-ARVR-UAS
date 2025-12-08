using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Ray Settings")]
    public Camera cam;
    public float rayDistance = 3f;
    public LayerMask interactLayer;

    [Header("UI Reference")]
    public CrosshairController crosshair;

    void Awake()
    {
        if (cam == null) cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Safety Check
        if (cam == null || UIPlayerManager.Instance == null) return;

        // --- PENCEGAH BUG ---
        // Jika sedang dialog, Matikan Raycast. 
        // Biarkan UIPlayerManager yang menangani tombol 'E' untuk "Next Text".
        // Kalau baris ini tidak ada, tombol E akan bentrok (Start vs Next).
        if (UIPlayerManager.Instance.isDialogueActive) 
        {
            // Pastikan prompt interaksi hilang saat dialog
            UIPlayerManager.Instance.HideInteraction();
            if (crosshair != null) crosshair.SetInteractable(false);
            return; 
        }

        RaycastCheck();
    }

    void RaycastCheck()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        bool isLookingAtInteractable = false;

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer))
        {
            // Ambil script InteractableObject (Induk dari NPCInteractable)
            InteractableObject obj = hit.collider.GetComponentInParent<InteractableObject>();

            if (obj != null)
            {
                isLookingAtInteractable = true;

                if (obj.isCanInteract())
                {
                    UIPlayerManager.Instance.ShowInteraction("Tekan [E] untuk " + obj.interactionName);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Reset UI prompt sebelum memulai interaksi
                        UIPlayerManager.Instance.HideInteraction();
                        if (crosshair != null) crosshair.SetInteractable(false);

                        obj.Interact();
                        return;
                    }
                }
            }
        }

        // Jika tidak melihat apa-apa
        if (!isLookingAtInteractable)
        {
            UIPlayerManager.Instance.HideInteraction();
        }

        if (crosshair != null)
        {
            crosshair.SetInteractable(isLookingAtInteractable);
        }
    }
}