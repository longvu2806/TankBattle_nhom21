using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Tài sản")]
    public int currentCoin = 1000; // Tiền khởi đầu (nếu chưa có dữ liệu)

    [Header("Kho đồ (ID các xe đã mua)")]
    public List<int> ownedTankIds = new List<int>();

    [Header("Trang bị Hiện tại")]
    public int equippedTankId = 0;

    private void Awake()
    {
        // Singleton: Giữ cho script này sống mãi qua các màn chơi
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

    private void Start()
    {
        // Ngay khi game bật, tải dữ liệu từ máy lên
        LoadInventoryData();
    }

    // --- [MỚI] HÀM 1: TẢI DỮ LIỆU TỪ MÁY TÍNH ---
    void LoadInventoryData()
    {
        // 1. Tải Tiền
        if (PlayerPrefs.HasKey("UserGold"))
        {
            currentCoin = PlayerPrefs.GetInt("UserGold");
        }
        else
        {
            currentCoin = 1000; // Người mới tặng 1000 vàng
            PlayerPrefs.SetInt("UserGold", currentCoin);
        }

        // 2. Tải xe đang chọn
        equippedTankId = PlayerPrefs.GetInt("SelectedTankID", 0);

        // 3. Tải danh sách xe đã mua
        // Mặc định luôn cho sở hữu xe số 0 (Xe Basic) để tránh lỗi
        if (!ownedTankIds.Contains(0)) ownedTankIds.Add(0);
        PlayerPrefs.SetInt("Tank_0", 1); // Đánh dấu chắc chắn đã có xe 0
    }

    // --- HÀM 2: KIỂM TRA SỞ HỮU ---
    public bool HasTank(int id)
    {
        // Kiểm tra trong bộ nhớ máy (PlayerPrefs)
        // Nếu key "Tank_ID" có giá trị 1 nghĩa là đã mua.
        // Hoặc kiểm tra trong List tạm thời.
        return PlayerPrefs.GetInt("Tank_" + id, 0) == 1 || ownedTankIds.Contains(id);
    }

    // --- HÀM 3: MUA XE (VÀ LƯU NGAY LẬP TỨC) ---
    public bool BuyTank(TankData tank)
    {
        if (HasTank(tank.id)) return false; // Đã có rồi thì thôi

        if (currentCoin >= tank.price)
        {
            // 1. Trừ tiền
            currentCoin -= tank.price;

            // 2. Lưu Tiền Mới vào máy
            PlayerPrefs.SetInt("UserGold", currentCoin);

            // 3. Lưu Xe Mới (Đánh dấu Tank_ID = 1)
            PlayerPrefs.SetInt("Tank_" + tank.id, 1);

            // 4. Lưu lại toàn bộ thay đổi xuống ổ cứng
            PlayerPrefs.Save();

            // 5. Cập nhật list hiển thị (để bạn nhìn thấy trên Inspector)
            if (!ownedTankIds.Contains(tank.id)) ownedTankIds.Add(tank.id);

            Debug.Log($"MUA THÀNH CÔNG: {tank.tankName} - Còn lại: {currentCoin}");
            return true;
        }

        Debug.Log("KHÔNG ĐỦ TIỀN!");
        return false;
    }

    // --- HÀM 4: TRANG BỊ XE (VÀ LƯU) ---
    public void EquipTank(int id)
    {
        if (HasTank(id))
        {
            equippedTankId = id;

            // Lưu lựa chọn vào máy
            PlayerPrefs.SetInt("SelectedTankID", id);
            PlayerPrefs.Save();

            Debug.Log($"Đã trang bị và lưu xe số: {id}");
        }
        else
        {
            Debug.Log($"LỖI: Bạn chưa mua xe số {id} nên không thể trang bị!");
        }
    }

    // --- [MỚI] HÀM 5: NẠP TIỀN (Dùng cho GameManager gọi khi thắng trận) ---
    public void AddCoin(int amount)
    {
        currentCoin += amount;

        // Lưu ngay lập tức để tránh mất tiền
        PlayerPrefs.SetInt("UserGold", currentCoin);
        PlayerPrefs.Save();

        Debug.Log($"Đã cộng thêm {amount} vàng. Tổng hiện tại: {currentCoin}");
    }

    // --- GIỮ NGUYÊN CODE CỦA BẠN B (Hỗ trợ lấy ID nhanh) ---
    public int GetEquippedTankID()
    {
        return equippedTankId;
    }

    // --- GIỮ NGUYÊN CODE TEST CỦA BẠN B (Bấm phím tắt để test nhanh) ---
    private void Update()
    {
        // Bấm số 1 để chọn xe ID 0 (Basic)
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipTank(0);

        // Bấm số 2 để chọn xe ID 1 (Heavy - Lưu ý: Phải mua rồi mới bấm được nhé)
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipTank(1);
    }
}