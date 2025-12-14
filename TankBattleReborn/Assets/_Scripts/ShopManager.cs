using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    // Singleton để gọi ở đâu cũng được
    public static ShopManager Instance;

    [Header("Database")]
    // Danh sách tất cả các xe đang bán trong game
    public List<TankData> shopItems;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Nhiệm vụ ngày 8: Đọc List TankData
        LoadShopData();
    }

    void LoadShopData()
    {
        Debug.Log("Đang tải dữ liệu Shop...");
        // Sau này chúng ta sẽ code đoạn sinh ra UI (gửi sang cho Bạn A hiển thị)
        // Hiện tại chỉ cần log ra xem đã đọc được dữ liệu chưa
        foreach (var tank in shopItems)
        {
            Debug.Log($"Sản phẩm: {tank.tankName} - Giá: {tank.price}");
        }
    }

    // Hàm này để sau này UI bấm vào nút Mua sẽ gọi
    public TankData GetTankByID(int id)
    {
        return shopItems.Find(t => t.id == id);
    }
    // Thêm đoạn này vào cuối class ShopManager
    private void Update()
    {
        // Bấm phím B để thử mua cái xe số 1 (Tăng Hạng Nặng - Giá 500)
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Lấy dữ liệu xe số 1 từ danh sách
            TankData tankToBuy = GetTankByID(1);

            if (tankToBuy != null)
            {
                // Gọi sang Inventory để mua
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.BuyTank(tankToBuy);
                }
            }
        }
    }
}