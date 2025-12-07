using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color interactColor = Color.blue;

    public void SetInteractable(bool state)
    {
        crosshairImage.color = state ? interactColor : normalColor;
    }
}
