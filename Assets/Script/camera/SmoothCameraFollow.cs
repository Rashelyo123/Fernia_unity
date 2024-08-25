using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player;  // Referensi ke Transform player
    public float smoothSpeed = 0.125f;  // Kecepatan smoothing, nilai yang lebih rendah lebih halus
    public Vector3 offset;  // Offset antara kamera dan player

    void LateUpdate()
    {
        // Posisi target kamera berdasarkan posisi player dan offset
        Vector3 desiredPosition = player.position + offset;

        // Menginterpolasi antara posisi kamera saat ini dan posisi target
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Mengatur posisi kamera ke posisi yang sudah diinterpolasi
        transform.position = smoothedPosition;

        // Opsional: Jika ingin kamera tetap melihat ke arah player, gunakan ini
        // transform.LookAt(player);
    }
}
