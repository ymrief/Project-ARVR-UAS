using UnityEngine;
using TMPro;

public class NPCDialogUI : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public AudioClip openSfx;

    void Start() { if (canvas) canvas.enabled = false; }

    public void SetDialog(string n, string t)
    {
        if (nameText != null) nameText.text = n;
        if (dialogText != null) dialogText.text = t;
    }

    public void Show()
    {
        if (canvas != null) canvas.enabled = true;
        if (openSfx != null && AudioManager.I != null) AudioManager.I.PlaySFX(openSfx);
    }

    public void Hide() { if (canvas != null) canvas.enabled = false; }
}
