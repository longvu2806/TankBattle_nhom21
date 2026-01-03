using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopController : MonoBehaviour
{
    [Header("--- KẾT NỐI UI ---")]
    public Transform gridContent;   // Nơi chứa các thẻ bài
    public GameObject cardPrefab;   // Prefab ItemCardUI

    [Header("--- CHI TIẾT BÊN PHẢI ---")]
    public Image previewImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI loreText;   // Mô tả

    [Header("--- THANH CHỈ SỐ (SLIDERS) ---")]
    public Slider hpSlider;
    public Slider damageSlider;
    public Slider speedSlider;

    [Header("--- GACHA MODE UI ---")]
    public GameObject normalShopUI;  // Kéo cái Normal_Shop_UI vào đây
    public GameObject gachaUI;       // Kéo cái Gacha_UI vào đây
    public Image centerImage;        // Cái ảnh to ở giữa (dùng chung hoặc riêng)

    [Header("--- NÚT MUA ---")]
    public TextMeshProUGUI buyButtonText; // Text bên trong nút
    public Button buyButton;
    public TextMeshProUGUI totalGoldText; // Tiền hiện có

    [Header("--- DỮ LIỆU ---")]
    // Bạn kéo thả ItemData muốn bán vào đây (Cả Tank và Súng)
    public List<ItemData> shopItems;
    private bool isGachaMode = false;
    private List<ItemCardUI> spawnedCards = new List<ItemCardUI>();
    private int currentSelectedIndex = -1;

    void Start()
    {
        // Tự động tìm nạp hàng hóa nếu list trống (Optional)
        if (shopItems.Count == 0 && InventoryManager.Instance != null)
        {
            shopItems = InventoryManager.Instance.allGameItems;
        }

        LoadShopItems();
        UpdateUI_Gold();
    }

    void Update()
    {
        UpdateUI_Gold(); // Cập nhật tiền liên tục
    }

    void UpdateUI_Gold()
    {
        if (InventoryManager.Instance != null && totalGoldText != null)
        {
            totalGoldText.text = InventoryManager.Instance.currentCoin.ToString();
        }
    }

    // 1. TẠO DANH SÁCH HÀNG
    void LoadShopItems()
    {
        // Xóa cũ
        foreach (Transform child in gridContent) Destroy(child.gameObject);
        spawnedCards.Clear();

        // Tạo mới
        for (int i = 0; i < shopItems.Count; i++)
        {
            GameObject newCardObj = Instantiate(cardPrefab, gridContent);
            ItemCardUI cardUI = newCardObj.GetComponent<ItemCardUI>();

            if (cardUI != null)
            {
                // Setup thẻ bài
                cardUI.Setup(i, this, shopItems[i]);
                spawnedCards.Add(cardUI);
            }
        }

        // Mặc định chọn món đầu tiên
        if (shopItems.Count > 0) OnCardSelected(0);
    }

    // 2. XỬ LÝ KHI BẤM CHỌN THẺ
    public void OnCardSelected(int index)
    {
        // Tắt viền chọn cái cũ
        if (currentSelectedIndex != -1 && currentSelectedIndex < spawnedCards.Count)
            spawnedCards[currentSelectedIndex].SetSelectState(false);

        currentSelectedIndex = index;

        // Bật viền chọn cái mới
        if (currentSelectedIndex < spawnedCards.Count)
            spawnedCards[currentSelectedIndex].SetSelectState(true);

        UpdateRightPanel(index);
    }

    // 3. CẬP NHẬT PANEL CHI TIẾT
    void UpdateRightPanel(int index)
    {
        if (index < 0 || index >= shopItems.Count) return;
        ItemData data = shopItems[index];

        // --- Hiển thị thông tin ---
        if (nameText) nameText.text = data.itemName;
        if (loreText) loreText.text = data.description;
        if (previewImage) previewImage.sprite = data.icon;

        // --- Hiển thị chỉ số (Slider) ---
        // Nếu là Súng -> Hiện Damge, Rate
        // Nếu là Tank -> Hiện HP, Speed
        if (hpSlider) hpSlider.value = data.healthBonus; // Max slider nên set là 2000
        if (damageSlider) damageSlider.value = data.damageBonus; // Max slider ~ 200
        if (speedSlider) speedSlider.value = data.moveSpeed;    // Max slider ~ 20

        // --- Kiểm tra trạng thái Mua/Trang bị ---
        if (InventoryManager.Instance != null)
        {
            bool isOwned = InventoryManager.Instance.CheckIfOwned(data);

            if (isOwned)
            {
                // Nếu đã có -> Kiểm tra xem có đang đeo không?
                bool isEquipped = false;
                if (data.itemType == ItemType.Hull && InventoryManager.Instance.equippedTankId == data.id) isEquipped = true;
                if (data.itemType == ItemType.Weapon && InventoryManager.Instance.equippedWeaponId == data.id) isEquipped = true;

                if (isEquipped)
                {
                    buyButtonText.text = "ĐANG DÙNG";
                    buyButton.interactable = false;
                }
                else
                {
                    buyButtonText.text = "TRANG BỊ";
                    buyButton.interactable = true;
                }
            }
            else
            {
                // Nếu chưa có -> Hiện giá mua
                buyButtonText.text = "MUA " + data.price + "$";
                buyButton.interactable = true;
            }
        }
    }

    // 4. HÀM GẮN VÀO NÚT MUA (BUY BUTTON)
    public void OnBuyBtnClicked()
    {
        if (isGachaMode)
        {
            // === CHẠY LOGIC GACHA ===
            // Gọi sang GachaSystem để xử lý trừ tiền, random đồ
            // GachaSystem.Instance.SpinGacha(); 
            Debug.Log("Đang quay Gacha...");
        }
        else
        {
            if (currentSelectedIndex == -1 || InventoryManager.Instance == null) return;

            ItemData data = shopItems[currentSelectedIndex];
            bool isOwned = InventoryManager.Instance.CheckIfOwned(data);

            if (isOwned)
            {
                // === LOGIC TRANG BỊ ===
                if (data.itemType == ItemType.Hull)
                    InventoryManager.Instance.equippedTankId = data.id;
                else if (data.itemType == ItemType.Weapon)
                    InventoryManager.Instance.equippedWeaponId = data.id;

                Debug.Log("Đã trang bị: " + data.itemName);
            }
            else
            {
                // === LOGIC MUA HÀNG ===
                bool success = InventoryManager.Instance.TryBuyItem(data);
                if (!success)
                {
                    // Có thể thêm hiệu ứng rung lắc nút hoặc hiện thông báo "Thiếu tiền"
                    Debug.Log("Thiếu tiền rồi đại ca ơi!");
                }
            }

            // Cập nhật lại giao diện sau khi bấm
            UpdateRightPanel(currentSelectedIndex);
        }
    }

    //5 Hàm này gắn vào nút Tab GACHA
    public void SwitchToGachaMode()
    {
        isGachaMode = true;

        // 1. Đổi giao diện bên phải
        if (normalShopUI) normalShopUI.SetActive(false);
        if (gachaUI) gachaUI.SetActive(true);

        // 2. Load danh sách phần thưởng vào bên trái
        // Lấy list gacha từ InventoryManager
        shopItems = InventoryManager.Instance.gachaPool;
        LoadShopItems(); // Tận dụng hàm cũ để vẽ lại lưới!

        // 3. Đổi nút bấm
        buyButtonText.text = "QUAY (500$)";
        buyButton.interactable = true; // Luôn sáng nút

        // Reset lựa chọn
        currentSelectedIndex = -1;
    }

    //6 Hàm này gắn vào các Tab TANK/WEAPON...
    public void SwitchToShopMode(int typeIndex) // 0: Tank, 1: Weapon...
    {
        isGachaMode = false;

        // 1. Trả về giao diện shop
        if (normalShopUI) normalShopUI.SetActive(true);
        if (gachaUI) gachaUI.SetActive(false);

        // 2. Load list hàng bán (Logic lọc cũ của bạn)
        // ... (Code lọc list shopItems theo type) ...
        LoadShopItems();
    }
}