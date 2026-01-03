using System.Collections.Generic;

[System.Serializable] // Bắt buộc phải có dòng này để biến thành JSON được
public class PlayerData
{
    public int gold;                // Tiền vàng
    public int diamonds;            // Kim cương (nếu có)
    public List<int> ownedTankIDs;  // Danh sách ID các xe đã mua
    public int currentEquippedID;   // ID xe đang trang bị

    // Hàm khởi tạo mặc định (cho người chơi mới)
    public PlayerData()
    {
        gold = 1000; // Cho 1000 vàng khởi nghiệp
        diamonds = 0;
        ownedTankIDs = new List<int>();
        ownedTankIDs.Add(0); // Mặc định sở hữu xe số 0 (Xe cơ bản)
        currentEquippedID = 0;
    }
}