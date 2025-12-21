using UnityEngine;

[CreateAssetMenu(fileName = "NewTankData", menuName = "Shop/Tank Data")]
public class TankData : ScriptableObject
{
    [Header("Thông tin cơ bản (Của B)")]
    public int id;
    public string tankName;
    public int price;
    public Sprite icon;         // B dùng tên là icon (Tôi dùng tankIcon) -> Ta sẽ theo B
    public float turnSpeed;

    [Header("UI Cần thêm (Của A)")]
    [TextArea] public string description; // Cốt truyện (B chưa có -> Thêm vào)
    public Sprite tankPreview;            // Ảnh 3D to (B chưa có -> Thêm vào)

    [Header("Thông số kỹ thuật")]
    public int health;          // B dùng int (Tôi dùng float) -> Theo B
    public float moveSpeed;     // B dùng moveSpeed (Tôi dùng speed) -> Theo B
    [Range(0, 100)] public float damage; // Sát thương (B chưa có -> Thêm vào)

    public GameObject tankPrefab;
    public bool isUnlocked;

    [Header("Giao diện (Thêm mới)")]
    // Kéo thanh trượt để chọn: 1=Xanh, 2=Tím, 3=Vàng
    [Range(0, 3)] public int rarityLevel;
}