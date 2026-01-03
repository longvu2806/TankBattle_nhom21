using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Chỉ số đạn")]
    public float speed = 15f;    // Tăng tốc lên chút cho sướng tay
    public int damage = 10;      // Sát thương
    public float lifeTime = 2f;  // Thời gian sống

    [Header("Hiệu ứng (Optional)")]
    public GameObject hitEffect; // Kéo Prefab vụ nổ vào đây (nếu có)

    void Start()
    {
        // Tự hủy sau 2 giây nếu không bắn trúng ai
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Di chuyển đạn
        // Lưu ý: Dùng Vector2.up là chuẩn nếu Sprite đạn của bạn hướng đầu lên trên
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. QUAN TRỌNG: Né chính người bắn (Player)
        if (other.CompareTag("Player")) return;

        // 2. [MỚI] Né các vùng Trigger khác (Ví dụ: Checkpoint, Item ăn tiền...)
        // Nếu cái kia cũng là Trigger thì đạn bay xuyên qua, không nổ
        if (other.isTrigger) return;

        // 3. Xử lý gây sát thương
        // Tìm xem vật bị bắn có script BaseTank (hoặc con của nó) không
        BaseTank tank = other.GetComponent<BaseTank>();
        if (tank != null)
        {
            tank.TakeDamage(damage);
        }

        // 4. [MỚI] Tạo hiệu ứng nổ (Nếu có)
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // 5. Hủy viên đạn
        Destroy(gameObject);
    }
}