using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint; // Vị trí sẽ sinh xe

    void Start()
    {
        // Khi game vừa chạy là sinh xe ngay
        SpawnTank();
    }

    void SpawnTank()
    {
        // 1. Lấy ID xe đang chọn (Mặc định là 0 nếu không tìm thấy Manager)
        int tankId = 0;
        if (InventoryManager.Instance != null)
        {
            tankId = InventoryManager.Instance.equippedTankId;
        }

        // 2. Tìm dữ liệu xe trong Shop dựa trên ID
        TankData data = null;
        if (ShopManager.Instance != null)
        {
            data = ShopManager.Instance.GetTankByID(tankId);
        }

        // 3. Nếu tìm thấy dữ liệu -> Sinh ra xe
        if (data != null && data.tankPrefab != null)
        {
            // Sinh ra xe tại vị trí StartPoint
            GameObject player = Instantiate(data.tankPrefab, spawnPoint.position, spawnPoint.rotation);

            // Đặt lại tên cho dễ quản lý (để các script khác tìm thấy)
            player.name = "PlayerTank";

            Debug.Log($"Đã sinh ra xe: {data.tankName} (ID: {tankId})");

            // Lấy script điều khiển của xe mới sinh ra
            TankController controller = player.GetComponent<TankController>();

            // Nạp dữ liệu vào cho nó
            if (controller != null)
            {
                controller.SetupTank(data);
                Debug.Log($"Đã nạp chỉ số: HP={data.health}, Speed={data.moveSpeed}");
            }
            // 4. (QUAN TRỌNG) Xử lý Camera bám theo
            // Vì xe cũ xóa rồi, Camera đang bơ vơ. Cần gán nó vào xe mới.
            if (Camera.main != null)
            {

                // Reset vị trí về giữa (giữ Z = -10 để nhìn thấy)
                Camera.main.transform.localPosition = new Vector3(0, 0, -10);
            }
        }
        else
        {
            Debug.LogError("LỖI: Không tìm thấy dữ liệu Tank hoặc chưa kéo Prefab vào TankData!");
        }
    }
}
