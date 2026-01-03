using UnityEngine;

public class TankController : BaseTank
{
    private Vector2 moveInput;
    private Camera mainCam;
    private Vector2 currentVelocity; // Biến dùng để tính toán độ trượt

    [Header("Cảm giác lái (Game Feel)")]
    public float turnSpeed = 200f;
    public float fireRate = 0.5f;

    [Header("Độ đầm của xe (Càng nhỏ càng trượt nhiều)")]
    public float acceleration = 10f; // Tăng tốc độ này để xe bốc hơn
    public float deceleration = 10f; // Giảm tốc độ này để xe phanh gấp hơn
    public int damage = 10;
    private float nextFireTime = 0f;

    protected override void Start()
    {
        base.Start();
        mainCam = Camera.main;
        gameObject.tag = "Player";
    }

    void Update()
    {
        // 1. Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // Chống đi chéo nhanh hơn đi thẳng

        RotateGunToMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        // --- LOGIC DI CHUYỂN MỚI (CÓ QUÁN TÍNH) ---
        if (moveInput.magnitude > 0)
        {
            // Đang bấm phím -> Tăng tốc từ từ
            currentVelocity = Vector2.MoveTowards(currentVelocity, moveInput * moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Nhả phím -> Giảm tốc từ từ (Trôi nhẹ)
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        rb.velocity = currentVelocity;

        // --- LOGIC XOAY THÂN XE (GIỮ NGUYÊN VÌ ĐÃ TỐT) ---
        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            targetAngle -= 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetupTank(ItemData data)
    {
        if (data == null) return;

        // 1. Nạp chỉ số RPG
        this.maxHealth = data.healthBonus;
        this.currentHealth = this.maxHealth; // Hồi đầy máu khi sinh ra

        // 2. Nạp chỉ số Vật Lý (Đây là phần Cách 2)
        // Kiểm tra > 0 để tránh trường hợp bạn quên nhập liệu làm xe đứng im
        if (data.moveSpeed > 0) this.moveSpeed = data.moveSpeed;
        if (data.turnSpeed > 0) this.turnSpeed = data.turnSpeed;
        if (data.acceleration > 0) this.acceleration = data.acceleration;

        // Nếu xe bạn có script bắn súng riêng (VD: ShootingController), 
        // bạn có thể gọi nó để cập nhật Damage tại đây luôn:
        this.damage = data.damageBonus;

        Debug.Log($"Đã setup xe {data.itemName}: Speed={moveSpeed}, Accel={acceleration}");
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
    protected override void Die()
    {
        // 1. Gọi GameManager xử lý THUA
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        // 2. Gọi hiệu ứng nổ và hủy object (từ class cha BaseTank)
        base.Die();
    }
}