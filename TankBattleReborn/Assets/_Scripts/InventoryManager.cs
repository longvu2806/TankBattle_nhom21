using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Tài sản")]
    public int currentCoin = 1000; // Cho sẵn 1000 vàng để test mua sắm

    [Header("Kho đồ")]
    // Lưu danh sách ID của những xe đã sở hữu (Ví dụ: 0, 1, 5...)
    public List<int> ownedTankIds = new List<int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Mặc định luôn cho sở hữu xe số 0 (Xe miễn phí)
        if (!ownedTankIds.Contains(0))
        {
            ownedTankIds.Add(0);
        }
    }

    // Hàm kiểm tra xem đã có xe này chưa
    public bool HasTank(int id)
    {
        return ownedTankIds.Contains(id);
    }

    // Hàm Mua xe (Logic quan trọng nhất của Ngày 9)
    public bool BuyTank(TankData tank)
    {
        // 1. Kiểm tra xem đã có chưa
        if (HasTank(tank.id))
        {
            Debug.Log("Bạn đã có xe này rồi!");
            return false; // Mua thất bại
        }

        // 2. Kiểm tra xem có đủ tiền không
        if (currentCoin >= tank.price)
        {
            // 3. Trừ tiền
            currentCoin -= tank.price;

            // 4. Thêm ID xe vào danh sách sở hữu
            ownedTankIds.Add(tank.id);

            Debug.Log($"MUA THÀNH CÔNG: {tank.tankName}. Số tiền còn lại: {currentCoin}");
            return true; // Mua thành công
        }
        else
        {
            Debug.Log("KHÔNG ĐỦ TIỀN! Cần: " + tank.price + " - Có: " + currentCoin);
            return false; // Mua thất bại
        }
    }
}