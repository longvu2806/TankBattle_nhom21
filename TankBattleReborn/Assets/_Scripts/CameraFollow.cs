using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Mục tiêu để bám theo (Xe tăng)
    public float smoothSpeed = 0.125f;  // Độ mượt (càng nhỏ càng trễ/mượt)
    public Vector3 offset;         // Khoảng cách giữ camera và xe (thường là Z = -10)

    void Start()
    {
        // Nếu chưa set offset tay thì tự động lấy khoảng cách hiện tại
        // Đảm bảo Camera đang ở vị trí Z = -10 trong Scene trước khi Play
        offset = transform.position - new Vector3(0, 0, 0);
        // Hoặc set cứng luôn cho chắc ăn:
        offset = new Vector3(0, 0, -10);
    }

    void LateUpdate() // Dùng LateUpdate để camera đi sau khi xe đã di chuyển xong
    {
        // 1. Nếu mất mục tiêu (xe nổ hoặc chưa sinh ra) -> Tự đi tìm
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
            return; // Chưa tìm thấy thì nghỉ, không chạy tiếp
        }

        // 2. Tính toán vị trí muốn đến
        // Chỉ lấy X, Y của xe + Offset. KHÔNG lấy Rotation.
        Vector3 desiredPosition = target.position + offset;

        // 3. Lướt tới đó một cách mượt mà (Lerp)
        // Vector3.Lerp(Vị trí hiện tại, Vị trí muốn đến, Độ mượt)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 4. Gán vị trí mới cho Camera
        transform.position = smoothedPosition;

        // 5. QUAN TRỌNG: Khóa cứng góc xoay (Không cho camera xoay theo xe)
        // Luôn nhìn thẳng tắp không nghiêng ngả
        transform.rotation = Quaternion.identity;
    }
}