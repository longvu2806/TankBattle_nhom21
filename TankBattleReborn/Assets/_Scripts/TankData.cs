using UnityEngine;

// Dòng này giúp chúng ta tạo file data ngay trong menu chuột phải của Unity
[CreateAssetMenu(fileName = "NewTankData", menuName = "Shop/Tank Data")]
public class TankData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public int id;              // Mã định danh (0, 1, 2...)
    public string tankName;     // Tên xe (Ví dụ: "Xe Tăng Cọp")
    public int price;           // Giá tiền
    public Sprite icon;         // Hình ảnh hiển thị trong Shop
    public float turnSpeed;

    [Header("Thông số kỹ thuật")]
    public int health;          // Máu
    public float moveSpeed;     // Tốc độ
    public GameObject tankPrefab; // Prefab thật để sinh ra trong game

    [Header("Trạng thái")]
    public bool isUnlocked;     // Đã mua chưa?
}