using UnityEngine;

// Kế thừa từ BaseTank thay vì MonoBehaviour
public class TankController : BaseTank
{
    private Vector2 moveInput;
    private Camera mainCam;

    // override: Ghi đè hàm Start của cha
    protected override void Start()
    {
        base.Start(); // Gọi logic của cha (lấy Rigidbody, set máu)
        mainCam = Camera.main;

        // Gán tag để đạn địch nhận diện
        gameObject.tag = "Player";
        // Cập nhật UI ngay khi vào game để hiển thị đúng 100/100
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
        }
    }

    void Update()
    {
        // 1. Input di chuyển
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. Xoay súng theo chuột
        RotateGunToMouse();

        // 3. Bắn (Gọi hàm Shoot của cha)
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // Di chuyển
        rb.velocity = moveInput.normalized * moveSpeed;

        // Xoay thân xe
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90;
        }
    }

    void RotateGunToMouse()
    {
        if (gunTurret != null)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - gunTurret.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gunTurret.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    // Ghi đè hàm chết: Player chết thì báo Game Over
    // Ghi đè hàm Die của cha (BaseTank)
    protected override void Die()
    {
        // 1. Gọi ông trọng tài báo Game Over
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        // 2. Sau đó mới gọi lệnh hủy xe của cha
        base.Die();
    }
}