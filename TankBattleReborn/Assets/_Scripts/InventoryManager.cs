using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> allGameItems = new List<ItemData>();
    public int currentCoin = 1000;

    // Sửa lại biến này để PlayerSpawner đỡ bị lỗi (Nếu PlayerSpawner cần int thì ta parse)
    public string equippedTankId = "tank_01";
    public string equippedWeaponId = "gun_01";
    public List<string> ownedTankIds = new List<string>();
    public List<ItemData> playerInventory = new List<ItemData>();
    public List<ItemData> ownedItems = new List<ItemData>();
    [Header("--- GACHA SYSTEM ---")]
    public List<ItemData> gachaPool = new List<ItemData>();

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);

        if (!ownedTankIds.Contains("tank_01")) ownedTankIds.Add("tank_01");
    }

    // --- CÁC HÀM GỐC (Dùng String) ---
    public bool HasTank(string id)
    {
        return ownedTankIds.Contains(id);
    }

    // Cho phép tham số cost là tùy chọn (mặc định = 0) để sửa lỗi ShopManager
    public void BuyTank(string id, int cost = 0)
    {
        if (currentCoin >= cost)
        {
            currentCoin -= cost;
            if (!ownedTankIds.Contains(id)) ownedTankIds.Add(id);
        }
    }

    public void EquipTank(string id)
    {
        equippedTankId = id;
    }

    // --- CÁC HÀM "PHIÊN DỊCH" (Sửa lỗi cho ShopController/Gacha dùng Int) ---
    // Tự động biến số thành string (VD: 1 -> "tank_01")

    public bool HasTank(int idIndex)
    {
        // Giả sử logic chuyển đổi: 0 -> tank_01, 1 -> tank_02...
        string idString = "tank_0" + (idIndex + 1);
        return HasTank(idString);
    }

    public void BuyTank(int idIndex, int cost = 0)
    {
        string idString = "tank_0" + (idIndex + 1);
        BuyTank(idString, cost);
    }

    public void BuyTank(int idIndex) // Trường hợp không truyền giá tiền
    {
        BuyTank(idIndex, 0);
    }

    public void EquipTank(int idIndex)
    {
        string idString = "tank_0" + (idIndex + 1);
        EquipTank(idString);
    }

    public void AddCoin(int amount) { currentCoin += amount; }
    public void AddItem(ItemData item) { if (!playerInventory.Contains(item)) playerInventory.Add(item); }
    // Hàm 1: Xử lý logic mua hàng (Trừ tiền, thêm đồ)
    public bool TryBuyItem(ItemData item)
    {
        // Kiểm tra tiền
        if (currentCoin >= item.price)
        {
            currentCoin -= item.price; // Trừ tiền
            ownedItems.Add(item);      // Thêm vào túi đồ

            Debug.Log($"Đã mua thành công: {item.itemName} - Giá: {item.price}");
            return true; // Báo lại là Mua thành công
        }
        else
        {
            Debug.Log("Không đủ tiền!");
            return false; // Báo thất bại
        }
    }

    // Hàm 2: Kiểm tra xem đã sở hữu món này chưa
    public bool CheckIfOwned(ItemData item)
    {
        // 1. Nếu là đồ mặc định (hull_01, gun_01) -> Luôn coi là đã có
        if (item.id == "hull_01" || item.id == "gun_01") return true;

        // 2. Kiểm tra trong danh sách
        return ownedItems.Contains(item);
    }
}