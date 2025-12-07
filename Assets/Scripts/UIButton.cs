using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Vector3 baseScale;
    public float hoverScale = 1.08f;
    public AudioClip clickSfx;
    void Awake() { baseScale = transform.localScale; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = baseScale * hoverScale;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = baseScale;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.I != null && clickSfx != null) AudioManager.I.PlaySFX(clickSfx);
    }
}
