using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemGuard : MonoBehaviour
{
    void Awake()
    {
        // ✅ Pastikan dia root
        transform.SetParent(null);

        EventSystem[] systems = FindObjectsOfType<EventSystem>();

        if (systems.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
