using UnityEngine;

public class ClimbingController : MonoBehaviour
{
    public float climbSpeed = 3f; // Kecepatan memanjat
    public LayerMask climbableLayer; // Layer untuk objek yang bisa dipanjat
    private CharacterController cc;
    private bool isClimbing = false;
    private bool canClimb = false; // Untuk mengecek apakah player bisa memanjat

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ketika player berada di dekat objek yang bisa dipanjat, tekan E untuk mulai memanjat
        if (canClimb && Input.GetKeyDown(KeyCode.E))
        {
            isClimbing = !isClimbing; // Tekan E sekali untuk memulai atau berhenti memanjat
        }

        if (isClimbing)
        {
            Climb();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Deteksi jika player berada di dekat objek yang bisa dipanjat
        if (other.CompareTag("Climbable")) // Gunakan tag untuk deteksi objek yang bisa dipanjat
        {
            canClimb = true;
            Debug.Log("Collide with climbable object!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Jika player meninggalkan area objek yang bisa dipanjat
        if (other.CompareTag("Climbable"))
        {
            canClimb = false;
            isClimbing = false; // Otomatis berhenti memanjat saat meninggalkan area
        }
    }

    private void Climb()
    {
        // Mendapatkan input vertikal dari player untuk memanjat
        float verticalInput = Input.GetAxis("Vertical");

        // Menggerakkan player ke atas atau bawah sesuai input
        if (verticalInput != 0)
        {
            Vector3 climbDirection = new Vector3(0, verticalInput * climbSpeed * Time.deltaTime, 0);
            cc.Move(climbDirection);
        }
    }
}
