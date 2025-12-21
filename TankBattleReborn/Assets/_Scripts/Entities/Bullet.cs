using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;     // Sát thương của đạn
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Tự hủy nếu bắn trượt
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Nếu đạn trúng vật thể có gắn thẻ "Player", bỏ qua (để không tự bắn mình)
        if (other.CompareTag("Player")) return;

        // 2. Kiểm tra xem vật thể bị bắn trúng có máu (EnemyHealth) không?
        BaseTank tank = other.GetComponent<BaseTank>();

        if (tank != null)
        {
            // Gọi hàm trừ máu bên script EnemyHealth
            tank.TakeDamage(damage);
        }

        // 3. Hủy viên đạn (dù trúng tường hay trúng địch đều mất đạn)
        Destroy(gameObject);
    }
}