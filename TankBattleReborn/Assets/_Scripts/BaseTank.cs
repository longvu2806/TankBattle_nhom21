using UnityEngine;

// abstract nghĩa là class này chỉ làm khuôn, không gắn trực tiếp vào game object nào cả
public abstract class BaseTank : MonoBehaviour
{
    [Header("Chỉ số chung")]
    public float moveSpeed = 5f;
    public int maxHealth = 100;
    protected int currentHealth; // protected: Chỉ con cái mới được dùng

    [Header("Vũ khí")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform gunTurret;

    protected Rigidbody2D rb;

    // virtual: Cho phép lớp con ghi đè (sửa đổi) logic
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Hàm nhận sát thương (Dùng chung cho cả Ta và Địch)
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " còn: " + currentHealth + " máu.");

        // --- ĐOẠN MỚI: CẬP NHẬT UI ---
        // Chỉ cập nhật UI nếu đây là Player (xe người chơi)
        if (gameObject.CompareTag("Player"))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthUI(currentHealth,maxHealth);
            }
        }
        // -----------------------------

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hàm bắn (Dùng chung)
    protected virtual void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, gunTurret.rotation);
        }
    }

    // Hàm chết (Mỗi thằng chết một kiểu nên để abstract hoặc virtual)
    protected virtual void Die()
    {
        // Chỉ đơn giản là hủy object, không gọi UI gì cả
        Debug.Log(gameObject.name + " đã bị tiêu diệt!");
        Destroy(gameObject);
    }
}
