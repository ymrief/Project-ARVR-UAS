using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public static class Haptic
{
    public static void TriggerHaptics(float amplitude = 0.2f, float duration = 0.05f)
    {
        SendHapticToNode(XRNode.LeftHand, amplitude, duration);
        SendHapticToNode(XRNode.RightHand, amplitude, duration);
    }

    static void SendHapticToNode(XRNode node, float amp, float dur)
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(node, devices);
        foreach (var d in devices)
        {
            if (d.isValid && d.TryGetHapticCapabilities(out HapticCapabilities caps))
            {
                if (caps.supportsImpulse) d.SendHapticImpulse(0, amp, dur);
            }
        }
    }
}
