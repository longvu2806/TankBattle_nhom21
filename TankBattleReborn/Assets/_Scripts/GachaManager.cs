using UnityEngine;
using System.Collections.Generic; // Để dùng List

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    [Header("Cài đặt Gacha")]
    public int spinCost = 200;

    // [SỬA 1]: Kéo ItemData trực tiếp vào đây (vì ShopManager có thể chưa sẵn sàng)
    public List<ItemData> gachaPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Spin()
    {
        if (InventoryManager.Instance.currentCoin >= spinCost)
        {
            InventoryManager.Instance.currentCoin -= spinCost;
            Debug.Log($"Đã trừ {spinCost} vàng. Đang quay...");

            // [SỬA 2]: Dùng danh sách gachaPool nội bộ
            if (gachaPool == null || gachaPool.Count == 0)
            {
                Debug.LogError("Chưa có đồ trong Gacha Pool!");
                return;
            }

            int randomIndex = Random.Range(0, gachaPool.Count);
            ItemData reward = gachaPool[randomIndex];

            ProcessReward(reward);
        }
        else
        {
            Debug.Log("KHÔNG ĐỦ TIỀN!");
        }
    }

    void ProcessReward(ItemData reward)
    {
        // [SỬA 3]: Truyền reward.id (String) vào hàm HasTank
        bool alreadyOwned = InventoryManager.Instance.HasTank(reward.id);

        if (alreadyOwned)
        {
            int refund = 50;
            InventoryManager.Instance.currentCoin += refund;
            Debug.Log($"Đã có {reward.itemName} -> Hoàn lại {refund} vàng.");
        }
        else
        {
            // [SỬA 4]: Thêm ID vào danh sách sở hữu
            InventoryManager.Instance.ownedTankIds.Add(reward.id);
            Debug.Log($"CHÚC MỪNG! BẠN NHẬN ĐƯỢC: {reward.itemName}");
        }
    }

    // Test
    private void Update() { if (Input.GetKeyDown(KeyCode.G)) Spin(); }
}