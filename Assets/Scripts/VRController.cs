using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float jumpForce = 6f;

    [Header("Space Gravity")]
    public float gravity = 4f; // LOW gravity

    [Header("View")]
    public float lookSpeed = 2f;
    public Camera cam;

    float yVelocity;
    float rotationX;
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed * 100 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * 100 * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80, 80);

        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;

        if (cc.isGrounded)
        {
            if (yVelocity < 0) yVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
                yVelocity = jumpForce;
        }

        yVelocity -= gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed;
        velocity.y = yVelocity;

        cc.Move(velocity * Time.deltaTime);
    }
}
