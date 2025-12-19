using UnityEngine;

// Kế thừa từ BaseTank
public class TankController : BaseTank
{
    private Vector2 moveInput;
    private Camera mainCam;

    [Header("Movement Settings")]
    public float turnSpeed = 200f; // Tốc độ xoay (độ/giây). Bạn chỉnh số này trong Inspector nếu muốn xoay nhanh hơn

    // override: Ghi đè hàm Start của cha
    protected override void Start()
    {
        base.Start(); // Gọi logic của cha (lấy Rigidbody, set máu)
        mainCam = Camera.main;

        // Gán tag để đạn địch nhận diện
        gameObject.tag = "Player";

        // Cập nhật UI ngay khi vào game
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
        // --- PHẦN SỬA ĐỔI: XOAY 360 ĐỘ ---

        if (moveInput != Vector2.zero)
        {
            // 1. Di chuyển xe
            rb.velocity = moveInput.normalized * moveSpeed;

            // 2. Tính góc cần xoay tới (360 độ)
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            targetAngle -= 90; // Trừ 90 độ cho khớp với Sprite hướng lên

            // 3. Xoay từ từ (Smooth) thay vì gán trực tiếp
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Dừng xe khi không bấm phím
            rb.velocity = Vector2.zero;
        }
    }

    public void SetupTank(TankData data)
    {
        // Gán chỉ số từ Data vào biến của xe
        maxHealth = data.health; // Lưu ý: Trong Data bạn đặt tên là maxHealth hay health thì sửa lại cho khớp nhé
        currentHealth = maxHealth;
        moveSpeed = data.moveSpeed;

        // Nếu trong file TankData của bạn đã thêm biến turnSpeed thì bỏ comment dòng dưới để lấy từ data
        // turnSpeed = data.turnSpeed; 

        // (Nếu xe bạn có biến damage thì gán luôn: damage = data.damage)

        // Cập nhật lại UI ngay lập tức
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthUI((int)currentHealth, (int)maxHealth);
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