using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Tài sản")]
    public int currentCoin = 1000;

    [Header("Kho đồ")]
    public List<int> ownedTankIds = new List<int>();

    // Biến lưu ID xe đang chọn (Mặc định là 0 - Xe cơ bản)
    [Header("Trang bị Hiện tại")]
    public int equippedTankId = 0;

    private void Awake()
    {
        // Singleton: Giữ sống qua các màn chơi
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ script này khi chuyển Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // --- CẬP NHẬT MỚI: Load lại xe đã chọn từ lần chơi trước ---
        // Nếu chưa lưu gì thì mặc định lấy số 0
        equippedTankId = PlayerPrefs.GetInt("SelectedTankID", 0);
        // -----------------------------------------------------------

        // Luôn sở hữu xe mặc định (ID 0) để tránh lỗi
        if (!ownedTankIds.Contains(0)) ownedTankIds.Add(0);
    }

    // Hàm kiểm tra sở hữu
    public bool HasTank(int id)
    {
        return ownedTankIds.Contains(id);
    }

    // Hàm mua xe
    public bool BuyTank(TankData tank)
    {
        if (HasTank(tank.id)) return false;

        if (currentCoin >= tank.price)
        {
            currentCoin -= tank.price;
            ownedTankIds.Add(tank.id);
            Debug.Log($"MUA THÀNH CÔNG: {tank.tankName}");
            return true;
        }
        return false;
    }

    // --- CẬP NHẬT MỚI: Hàm Chọn xe (Equip) có Lưu trữ ---
    public void EquipTank(int id)
    {
        // 1. Phải kiểm tra xem có xe đó chưa thì mới cho lái
        if (HasTank(id))
        {
            equippedTankId = id; // Cập nhật biến tạm thời

            // --- LƯU VÀO MÁY TÍNH ---
            PlayerPrefs.SetInt("SelectedTankID", id);
            PlayerPrefs.Save(); // Lưu ngay lập tức
            // ------------------------

            Debug.Log($"---> ĐÃ TRANG BỊ VÀ LƯU XE SỐ: {id}");
        }
        else
        {
            Debug.Log($"LỖI: Bạn chưa mua xe số {id} nên không thể trang bị!");
        }
    }

    // Hàm phụ trợ để các script khác (như PlayerSpawner) lấy ID nhanh
    public int GetEquippedTankID()
    {
        return equippedTankId;
    }

    // --- Test nhanh bằng phím tắt ---
    private void Update()
    {
        // Giả sử bạn đã "mua" xe số 1 rồi (add tay vào list ownedTankIds trên Inspector để test)

        // Bấm số 1 để chọn xe ID 0
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipTank(0);

        // Bấm số 2 để chọn xe ID 1
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipTank(1);
    }
}