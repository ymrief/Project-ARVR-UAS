using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color interactColor = Color.red;

    void OnEnable()
    {
        SetInteractable(false); // Paksa reset jadi putih saat game mulai/pindah scene
    }

    public void SetInteractable(bool state)
    {
        if (crosshairImage == null) return;

        if (state)
        {
            crosshairImage.color = interactColor;
        }
        else
        {
            crosshairImage.color = normalColor;
        }
    }
}
