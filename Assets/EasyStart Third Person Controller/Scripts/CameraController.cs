using UnityEngine;

public class CameraController2_5D : MonoBehaviour
{
    [Tooltip("Kecepatan kamera mengikuti player.")]
    public float followSpeed = 2f;
    
    [Tooltip("Jarak tetap kamera dari player di sepanjang sumbu Z.")]
    public float fixedZOffset = -10f;
    
    [Tooltip("Jarak tetap kamera dari player di sepanjang sumbu Y.")]
    public float fixedYOffset = 5f;
    
    [Tooltip("Seberapa halus kamera mengikuti pemain.")]
    public float smoothness = 0.125f;
    
    private Transform player;

    void Start()
    {
        // Mencari player berdasarkan tag
        player = GameObject.FindWithTag("Player").transform;
        
        if (player == null)
        {
            Debug.LogError("Player dengan tag 'Player' tidak ditemukan!");
        }
    }

    void LateUpdate()
{
    if (player != null)
    {
        // Posisi kamera mengikuti sumbu X, Y, dan Z player dengan offset tetap
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y + fixedYOffset, player.position.z + fixedZOffset);
        
        // Gerakan kamera yang smooth
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothness);
        transform.position = smoothedPosition;

        // Kamera tetap menghadap ke arah player
        transform.LookAt(player);
    }
}

}
