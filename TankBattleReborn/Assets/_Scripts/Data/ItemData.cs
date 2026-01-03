using UnityEngine;

// Tạo menu chuột phải: Create -> Inventory -> Item Data
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("--- THÔNG TIN CƠ BẢN ---")]
    public string id;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("--- PHÂN LOẠI & GIÁ ---")]
    public ItemType itemType;
    public Rarity rarity;
    public int price;

    [Header("--- PREFABS (QUAN TRỌNG) ---")]
    // Nếu là Hull -> Kéo Prefab Thân xe vào đây
    public GameObject hullPrefab;

    // Nếu là Weapon -> Kéo Prefab Súng vào đây
    public GameObject weaponPrefab;

    [Header("--- CHỈ SỐ THÂN XE (HULL STATS) ---")]
    public int healthBonus;      // Máu cộng thêm
    public float moveSpeed;      // Tốc độ di chuyển
    public float turnSpeed;      // Tốc độ xoay thân xe
    public float acceleration;   // Gia tốc (Độ bốc)

    [Header("--- CHỈ SỐ VŨ KHÍ (WEAPON STATS) ---")]
    public int damageBonus;      // Sát thương mỗi viên đạn
    public float fireRate;       // Tốc độ bắn (Giây/viên). Vd: 0.5 là bắn 2 viên/giây
}

// ---------------------------------------------------------
// DANH MỤC ITEM
// ---------------------------------------------------------

public enum ItemType
{
    Hull,           // Thân xe (Khung gầm)
    Weapon,         // Vũ khí (Súng)
    Currency,       // Tiền (Vàng, Gem)
    Material,       // Nguyên liệu
    Consumable      // Đồ tiêu thụ
}

public enum Rarity
{
    Common,         // Thường (Xám)
    Rare,           // Hiếm (Xanh dương)
    Epic,           // Sử thi (Tím)
    Legendary       // Huyền thoại (Cam)
}