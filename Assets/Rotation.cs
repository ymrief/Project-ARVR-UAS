using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        // Rotasi pada sumbu Y
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
