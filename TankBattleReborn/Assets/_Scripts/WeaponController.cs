using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("--- CÀI ĐẶT SÚNG ---")]
    public float rotateSpeed = 10f;      // Tốc độ xoay nòng súng
    public float fireRate = 0.5f;        // Tốc độ bắn (càng nhỏ bắn càng nhanh)
    public int damage = 10;              // Sát thương (sẽ được ghi đè bởi Data)

    [Header("--- KẾT NỐI ---")]
    public GameObject bulletPrefab;      // Kéo Prefab viên đạn vào đây
    public Transform firePoint;          // Kéo cái chấm đầu nòng vào đây

    private float nextFireTime = 0f;     // Biến đếm thời gian
    private Camera mainCam;

    void Start()
    {
        // Lấy Camera chính để tính vị trí chuột
        mainCam = Camera.main;
    }

    void Update()
    {
        // 1. Xoay súng
        RotateGunToMouse();

        // 2. Bắn súng
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void RotateGunToMouse()
    {
        if (mainCam == null) return;

        // Lấy vị trí chuột trong thế giới game
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Tính hướng từ súng tới chuột
        Vector2 direction = mousePos - transform.position;

        // Tính góc xoay (Atan2 trả về radian -> đổi sang độ)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Trừ 90 độ vì sprite súng của bạn đang hướng lên trên (Up)
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);

        // Xoay từ từ cho mượt (Lerp)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Chưa gắn Bullet Prefab hoặc Fire Point cho súng!");
            return;
        }

        // Tạo viên đạn tại đầu nòng
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // -- ĐOẠN NÀY ĐỂ GÁN DAMAGE CHO ĐẠN (Nếu bạn đã có script BulletController) --
        // BulletController bulletScript = bullet.GetComponent<BulletController>();
        // if (bulletScript != null)
        // {
        //     bulletScript.damage = this.damage; // Truyền sát thương của súng sang đạn
        // }
    }

    // Hàm này để InventoryManager gọi khi lắp súng (để update chỉ số xịn hơn)
    public void SetupWeapon(ItemData data)
    {
        this.damage = data.damageBonus;
        // this.fireRate = data.fireRate; // Nếu trong Data có biến fireRate
    }
}