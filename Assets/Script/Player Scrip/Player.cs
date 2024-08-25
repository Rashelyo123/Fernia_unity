using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float climbSpeed = 3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask climbableWallMask;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Joystick joystick; // Tambahkan referensi ke Joystick UI

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isClimbing;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Cek apakah player menyentuh tanah
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
        Debug.Log("IsGrounded: " + isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Menjaga player tetap di tanah dengan nilai kecil
        }

        // Cek apakah ada dinding yang bisa dipanjat di depan
        Vector3 capsuleBottom = transform.position + controller.center - Vector3.up * (controller.height / 2);
        Vector3 capsuleTop = transform.position + controller.center + Vector3.up * (controller.height / 2);

        isClimbing = Physics.CapsuleCast(capsuleBottom, capsuleTop, controller.radius, transform.forward, out RaycastHit hit, wallCheckDistance, climbableWallMask);
        Debug.Log(isClimbing ? "Dinding terdeteksi: " + hit.collider.name : "Tidak ada dinding terdeteksi.");

        // Input gerakan dari joystick UI
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        // Move the character
        controller.Move(move * speed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Climbing logic
        if (isClimbing && Input.GetButton("Jump")) // Menggunakan tombol lompat untuk memanjat
        {
            velocity.y = climbSpeed; // Buat player naik dengan kecepatan panjat
        }

        // Penerapan gravitasi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
