using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 10;
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đụng trúng Người chơi (Player)
        if (other.CompareTag("Player"))
        {
            // Thay vì tìm PlayerHealth, giờ ta tìm BaseTank (Class cha)
            BaseTank playerTank = other.GetComponent<BaseTank>();

            if (playerTank != null)
            {
                playerTank.TakeDamage(damage); // Gọi hàm trừ máu chung
            }
            Destroy(gameObject); // Hủy đạn
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}