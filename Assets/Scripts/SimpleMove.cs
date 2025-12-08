using UnityEngine;
public class SimpleMove : MonoBehaviour {
    public float moveSpeed = 10f;
    void Update() {
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(x, 0, z);

        if (Input.GetMouseButton(1)) { // Tahan Klik Kanan untuk tengok
            transform.Rotate(0, Input.GetAxis("Mouse X") * 2f, 0);
        }
    }
}