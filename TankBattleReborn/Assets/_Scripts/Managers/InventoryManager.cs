using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // --- [CHIÊU THỨC BẮC CẦU] ---
    // Các biến này giúp GachaManager và ShopController không bị lỗi
    // Nó tự động lấy dữ liệu từ GameManager chứ không lưu lẻ tẻ nữa
    public int currentCoin
    {
        get { return GameManager.Instance.playerData.gold; }
        set
        {
            GameManager.Instance.playerData.gold = value;
            GameManager.Instance.SaveGame(); // Tự động lưu khi thay đổi
        }
    }

    public int equippedTankId
    {
        get { return GameManager.Instance.playerData.currentEquippedID; }
        set
        {
            GameManager.Instance.playerData.currentEquippedID = value;
            GameManager.Instance.SaveGame();
        }
    }

    // List này dùng getter để trả về list thật bên GameManager
    public List<int> ownedTankIds
    {
        get { return GameManager.Instance.playerData.ownedTankIDs; }
    }
    // -----------------------------

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- HÀM 1: KIỂM TRA SỞ HỮU ---
    public bool HasTank(int id)
    {
        return GameManager.Instance.playerData.ownedTankIDs.Contains(id);
    }

    // --- HÀM 2: MUA XE ---
    public bool BuyTank(TankData tank)
    {
        if (HasTank(tank.id)) return false;

        // Dùng biến currentCoin (nó sẽ tự trừ bên GameManager nhờ cái cầu bên trên)
        if (currentCoin >= tank.price)
        {
            currentCoin -= tank.price; // Trừ tiền
            GameManager.Instance.playerData.ownedTankIDs.Add(tank.id); // Thêm xe
            GameManager.Instance.SaveGame(); // Lưu

            Debug.Log($"MUA THÀNH CÔNG: {tank.tankName}. Số dư mới: {currentCoin}");
            return true;
        }
        else
        {
            Debug.Log("KHÔNG ĐỦ TIỀN!");
            return false;
        }
    }

    // --- HÀM 3: TRANG BỊ XE ---
    public void EquipTank(int id)
    {
        if (HasTank(id))
        {
            equippedTankId = id; // Gán ID (nó tự lưu sang GameManager)
            Debug.Log($"Đã trang bị xe số: {id}");
        }
        else
        {
            Debug.Log("Chưa mua xe này!");
        }
    }

    // --- HÀM 4: NẠP TIỀN ---
    public void AddCoin(int amount)
    {
        currentCoin += amount; // Cộng tiền
        Debug.Log($"Đã cộng {amount} vàng. Tổng: {currentCoin}");
    }

    // --- Test ---
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipTank(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipTank(1);
        if (Input.GetKeyDown(KeyCode.M)) AddCoin(1000);
    }
}