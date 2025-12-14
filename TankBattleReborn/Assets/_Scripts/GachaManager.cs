using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    [Header("Cài đặt Gacha")]
    public int spinCost = 200; // Giá mỗi lần quay (200 vàng)

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Hàm Quay thưởng (Logic chính của Ngày 10)
    public void Spin()
    {
        // 1. Kiểm tra tiền (Gọi sang ví của Inventory)
        if (InventoryManager.Instance.currentCoin >= spinCost)
        {
            // 2. Trừ tiền
            InventoryManager.Instance.currentCoin -= spinCost;
            Debug.Log($"Đã trừ {spinCost} vàng. Đang quay...");

            // 3. Logic Random (Quan trọng nhất)
            // Lấy danh sách tất cả xe đang bán trong Shop
            var allTanks = ShopManager.Instance.shopItems;

            // Random một số từ 0 đến độ dài danh sách
            int randomIndex = Random.Range(0, allTanks.Count);

            // Lấy ra phần thưởng
            TankData reward = allTanks[randomIndex];

            // 4. Xử lý phần thưởng
            ProcessReward(reward);
        }
        else
        {
            Debug.Log("KHÔNG ĐỦ TIỀN ĐỂ QUAY! Cần: " + spinCost);
        }
    }

    void ProcessReward(TankData reward)
    {
        // Kiểm tra xem đã có xe này chưa
        bool alreadyOwned = InventoryManager.Instance.HasTank(reward.id);

        if (alreadyOwned)
        {
            // Nếu có rồi thì hoàn lại ít tiền an ủi (Cơ chế chống cay cú)
            int refund = 50;
            InventoryManager.Instance.currentCoin += refund;
            Debug.Log($"BẠN QUAY RA: {reward.tankName} (Đã có) -> Được hoàn lại {refund} vàng.");
        }
        else
        {
            // Nếu chưa có thì thêm vào kho
            InventoryManager.Instance.ownedTankIds.Add(reward.id);
            Debug.Log($"CHÚC MỪNG! BẠN NHẬN ĐƯỢC XE MỚI: {reward.tankName} !!!");
        }

        Debug.Log("Số dư hiện tại: " + InventoryManager.Instance.currentCoin);
    }

    // Test thử bằng phím G
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spin();
        }
    }
}