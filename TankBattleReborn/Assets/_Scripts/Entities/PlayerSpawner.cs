using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("--- CÀI ĐẶT ---")]
    public Transform spawnPoint; // Kéo vị trí StartPoint vào đây

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // 0. KIỂM TRA AN TOÀN
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("Thiếu InventoryManager trong Scene!");
            return;
        }

        // 1. LẤY ID TỪ KHO ĐỒ (ID xe và ID súng đang dùng)
        string hullID = InventoryManager.Instance.equippedTankId;

        // Nếu chưa có biến equippedWeaponId trong InventoryManager, hãy tạm thời dùng cứng "gun_01"
        // Hoặc thêm biến đó vào InventoryManager theo hướng dẫn trước
        string weaponID = InventoryManager.Instance.equippedWeaponId;
        if (string.IsNullOrEmpty(weaponID)) weaponID = "gun_01"; // Fallback nếu quên set

        // 2. TÌM DATA ITEM (Dựa vào ID)
        ItemData hullData = FindItemData(hullID);
        ItemData weaponData = FindItemData(weaponID);

        // 3. TIẾN HÀNH LẮP RÁP
        if (hullData != null && hullData.hullPrefab != null)
        {
            // --- A. TẠO THÂN XE (HULL) ---
            GameObject playerTank = Instantiate(hullData.hullPrefab, spawnPoint.position, spawnPoint.rotation);
            playerTank.name = "PlayerTank";

            // Nạp chỉ số thân xe (Máu, Tốc độ...)
            var tankCtrl = playerTank.GetComponent<TankController>();
            if (tankCtrl != null) tankCtrl.SetupTank(hullData);

            // --- B. TÌM KHỚP GẮN SÚNG (GunMount) ---
            // Tìm object con có tên chính xác là "GunMount"
            Transform gunMount = playerTank.transform.Find("GunMount");

            if (gunMount != null && weaponData != null && weaponData.weaponPrefab != null)
            {
                // --- C. TẠO SÚNG (WEAPON) ---
                GameObject gun = Instantiate(weaponData.weaponPrefab, gunMount.position, gunMount.rotation);

                // QUAN TRỌNG: Gắn súng làm con của GunMount để nó dính liền với xe
                gun.transform.SetParent(gunMount);

                // Reset vị trí về 0 để súng nằm đúng tâm khớp
                gun.transform.localPosition = Vector3.zero;
                gun.transform.localRotation = Quaternion.identity;

                // Nạp chỉ số súng (Sát thương...)
                var weaponCtrl = gun.GetComponent<WeaponController>();
                if (weaponCtrl != null) weaponCtrl.SetupWeapon(weaponData);
            }
            else
            {
                if (gunMount == null) Debug.LogError("Lỗi: Không tìm thấy 'GunMount' trong Prefab thân xe!");
            }

            // --- D. CAMERA FOLLOW ---
            SetupCamera(playerTank.transform);
        }
        else
        {
            Debug.LogError($"Lỗi: Không tìm thấy Data Thân xe ({hullID}) hoặc chưa gắn Prefab!");
        }
    }

    // Hàm phụ: Tìm Data trong danh sách của InventoryManager
    ItemData FindItemData(string id)
    {
        foreach (var item in InventoryManager.Instance.allGameItems)
        {
            if (item.id == id) return item;
        }
        return null;
    }

    // Hàm phụ: Setup Camera
    void SetupCamera(Transform target)
    {
        if (Camera.main != null)
        {
            // Cách đơn giản: Di chuyển camera tới xe
            Camera.main.transform.position = new Vector3(target.position.x, target.position.y, -10);

            // Nếu bạn dùng script CameraFollow riêng, hãy bỏ comment dòng dưới:
            // var camScript = Camera.main.GetComponent<CameraFollow>();
            // if (camScript != null) camScript.target = target;
        }
    }
}