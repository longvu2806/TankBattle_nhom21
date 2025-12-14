using UnityEngine;

public class EnemyAI : BaseTank
{
    [Header("AI Setting")]
    public float detectionRange = 10f; // Tầm nhìn
    public float shootingRange = 7f;   // Tầm bắn
    public float fireRate = 1.5f;

    private float nextFireTime = 0f;
    private Transform playerTarget;

    protected override void Start()
    {
        base.Start();
        // Tìm player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerTarget = p.transform;

        // Gán tag
        gameObject.tag = "Enemy";
    }

    void Update()
    {
        if (playerTarget == null) return;

        float distance = Vector2.Distance(transform.position, playerTarget.position);

        // Logic AI: Đuổi theo
        if (distance < detectionRange)
        {
            // Di chuyển về phía player
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            // Xoay thân xe
            float angleBody = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angleBody - 90;

            // Xoay súng
            if (gunTurret != null)
            {
                Vector2 dirGun = playerTarget.position - gunTurret.position;
                float angleGun = Mathf.Atan2(dirGun.y, dirGun.x) * Mathf.Rad2Deg;
                gunTurret.rotation = Quaternion.Euler(0, 0, angleGun - 90);
            }

            // Logic AI: Bắn
            if (distance < shootingRange && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    // Ghi đè hàm Die của cha (BaseTank)
    protected override void Die()
    {
        // 1. Quan trọng nhất: Báo cho GameManager biết "Tôi đã chết, hãy trừ tôi đi!"
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDied();
        }

        // 2. Sau đó mới gọi lệnh nổ và biến mất của cha
        base.Die();
    }

    // Enemy chết thì chỉ cần nổ (Logic mặc định của cha là Destroy)
    // Không cần override Die() nếu không muốn làm gì đặc biệt
}