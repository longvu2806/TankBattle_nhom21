using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Database")]
    public List<TankData> shopItems; // Kéo hết TankData vào đây

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Debug kiểm tra xem đã kéo data chưa
        foreach (var tank in shopItems)
        {
            Debug.Log($"Shop đã tải: {tank.tankName} (ID: {tank.id}) - Giá: {tank.price}");
        }
    }

    // Hàm lấy thông tin xe theo ID
    public TankData GetTankByID(int id)
    {
        return shopItems.Find(t => t.id == id);
    }

    // [TEST NHANH] Update này để Bạn B test mua xe bằng phím tắt
    private void Update()
    {
        // Bấm B để mua xe số 1 (Ví dụ xe giá 500)
        if (Input.GetKeyDown(KeyCode.B))
        {
            TankData tankToBuy = GetTankByID(1); // Đảm bảo bạn có xe ID 1 trong list
            if (tankToBuy != null && InventoryManager.Instance != null)
            {
                InventoryManager.Instance.BuyTank(tankToBuy);
            }
        }
    }
}