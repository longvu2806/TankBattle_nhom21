using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Mục tiêu để bám theo (Xe tăng)
    public float smoothSpeed = 0.125f; // Độ mượt (Số càng nhỏ càng trôi chậm)
    public Vector3 offset;         // Khoảng cách lệch (nếu muốn)

    void FixedUpdate()
    {
        if (player != null)
        {
            // Xác định vị trí đích (Vị trí xe + độ lệch nếu có)
            // Giữ nguyên Z = -10 để không mất hình
            Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, -10f);

            // Hàm Lerp giúp camera trôi từ từ đến đích thay vì nhảy bụp một cái
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Cập nhật vị trí
            transform.position = smoothedPosition;
        }
    }
}