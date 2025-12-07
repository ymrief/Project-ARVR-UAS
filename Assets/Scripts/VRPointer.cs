using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRPointer : MonoBehaviour
{
    [Tooltip("Origin of the ray (controller or camera)")]
    public Transform rayOrigin;
    public float maxDistance = 10f;
    public LayerMask uiLayer = 1 << 5; 
    public LineRenderer line;
    public bool showLine = true;

    PointerEventData pointerData;
    GameObject currentHit;

    void Start()
    {
        if (EventSystem.current == null) Debug.LogError("EventSystem tidak ditemukan pada scene.");
        pointerData = new PointerEventData(EventSystem.current);
        if (line != null) line.positionCount = 2;
    }

    void Update()
    {
        if (rayOrigin == null) return;

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;
        bool didHit = Physics.Raycast(ray, out hit, maxDistance, uiLayer);

#if UNITY_EDITOR
        Debug.DrawRay(rayOrigin.position, rayOrigin.forward * maxDistance, didHit ? Color.green : Color.red);
#endif

        if (showLine && line != null)
        {
            line.enabled = true;
            line.SetPosition(0, rayOrigin.position);
            line.SetPosition(1, didHit ? hit.point : (rayOrigin.position + rayOrigin.forward * maxDistance));
        }

        if (didHit)
        {
            GameObject hitGO = hit.collider.gameObject;
            Camera cam = Camera.main;
            if (cam != null) pointerData.position = cam.WorldToScreenPoint(hit.point);

            if (hitGO != currentHit)
            {
                if (currentHit != null)
                    ExecuteEvents.Execute<IPointerExitHandler>(currentHit, pointerData, (x, y) => x.OnPointerExit(pointerData));

                ExecuteEvents.Execute<IPointerEnterHandler>(hitGO, pointerData, (x, y) => x.OnPointerEnter(pointerData));
                currentHit = hitGO;
            }

            bool click = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0);

#if UNITY_2019_1_OR_NEWER
            try
            {
                var devices = new List<UnityEngine.XR.InputDevice>();
                UnityEngine.XR.InputDevices.GetDevices(devices);
                foreach (var d in devices)
                {
                    if (d.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool pressed) && pressed) { click = true; break; }
                    if (d.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool p) && p) { click = true; break; }
                }
            }
            catch { }
#endif

            if (click && currentHit != null)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(currentHit, pointerData, (x, y) => x.OnPointerClick(pointerData));
            }
        }
        else
        {
            if (currentHit != null)
            {
                ExecuteEvents.Execute<IPointerExitHandler>(currentHit, pointerData, (x, y) => x.OnPointerExit(pointerData));
                currentHit = null;
            }
            if (line != null && !showLine) line.enabled = false;
        }
    }
}
