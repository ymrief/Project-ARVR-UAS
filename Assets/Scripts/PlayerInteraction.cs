using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Ray Settings")]
    public Camera cam;
    public float rayDistance = 3f;
    public LayerMask interactLayer;

    [Header("UI")]
    public CrosshairController crosshair;

    void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (cam == null || UIPlayerManager.Instance == null)
            return;

        RaycastCheck();
    }

    void RaycastCheck()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        bool isLookingAtInteractable = false;

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer))
        {
            InteractableObject obj =
                hit.collider.GetComponentInParent<InteractableObject>();

            if (obj != null)
            {
                isLookingAtInteractable = true;

                if (obj.isCanInteract())
                {
                    UIPlayerManager.Instance.ShowInteraction(
                        "Tekan [E] untuk " + obj.interactionName
                    );

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // ✅ RESET UI SEBELUM INTERACT
                        UIPlayerManager.Instance.HideInteraction();
                        crosshair.SetInteractable(false);

                        obj.Interact();
                        return;
                    }

                } 

            }
        }

        if (!isLookingAtInteractable)
        {
            UIPlayerManager.Instance.HideInteraction();
        }

        if (crosshair != null)
            crosshair.SetInteractable(isLookingAtInteractable);
    }
}
