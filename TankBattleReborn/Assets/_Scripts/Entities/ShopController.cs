using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopController : MonoBehaviour
{
    [Header("--- KẾT NỐI UI ---")]
    public Transform gridContent;
    public GameObject cardPrefab;

    [Header("--- CHI TIẾT BÊN PHẢI ---")]
    public Image previewImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI loreText;

    public Slider hpSlider;
    public Slider damageSlider;
    public Slider speedSlider;

    public TextMeshProUGUI buyButtonPrice; // Chữ trên nút mua
    public Button buyButton;               // Nút mua
    public TextMeshProUGUI totalGoldText;  // Hiển thị tổng tiền vàng

    [Header("--- DỮ LIỆU ---")]
    public List<TankData> tankList;

    private List<ItemCardUI> spawnedCards = new List<ItemCardUI>();
    private int currentSelectedIndex = -1;

    void Start()
    {
        UpdateUI(); // Hiển thị tiền ngay khi vào
        LoadShopItems();
    }

    void Update()
    {
        // Cập nhật tiền liên tục (đề phòng tiền thay đổi)
        UpdateUI();
    }

    void UpdateUI()
    {
        if (InventoryManager.Instance != null && totalGoldText != null)
        {
            totalGoldText.text = InventoryManager.Instance.currentCoin.ToString();
        }
    }

    void LoadShopItems()
    {
        foreach (Transform child in gridContent) Destroy(child.gameObject);
        spawnedCards.Clear();

        for (int i = 0; i < tankList.Count; i++)
        {
            GameObject newCardObj = Instantiate(cardPrefab, gridContent);
            ItemCardUI cardUI = newCardObj.GetComponent<ItemCardUI>();
            cardUI.Setup(i, this, tankList[i].tankName, tankList[i].price, tankList[i].icon, tankList[i].rarityLevel);
            spawnedCards.Add(cardUI);
        }

        if (tankList.Count > 0) OnCardSelected(0);
    }

    public void OnCardSelected(int index)
    {
        if (currentSelectedIndex != -1 && currentSelectedIndex < spawnedCards.Count)
            spawnedCards[currentSelectedIndex].SetSelectState(false);

        currentSelectedIndex = index;
        spawnedCards[index].SetSelectState(true);
        UpdateRightPanel(index);
    }

    void UpdateRightPanel(int index)
    {
        if (index < 0 || index >= tankList.Count) return;
        TankData data = tankList[index];

        if (nameText) nameText.text = data.tankName;
        if (loreText) loreText.text = data.description;
        if (previewImage) previewImage.sprite = (data.tankPreview != null) ? data.tankPreview : data.icon;

        if (damageSlider) damageSlider.value = data.damage;
        if (speedSlider) speedSlider.value = data.moveSpeed;
        if (hpSlider) hpSlider.value = data.health;

        // --- KIỂM TRA ĐÃ MUA CHƯA ---
        if (InventoryManager.Instance != null)
        {
            bool isOwned = InventoryManager.Instance.HasTank(data.id);
            if (isOwned)
            {
                if (buyButtonPrice) buyButtonPrice.text = "TRANG BỊ";
            }
            else
            {
                if (buyButtonPrice) buyButtonPrice.text = "MUA - " + data.price + "$";
            }
        }
    }

    // --- HÀM GẮN VÀO NÚT MUA ---
    public void OnBuyBtnClicked()
    {
        if (currentSelectedIndex == -1 || InventoryManager.Instance == null) return;
        TankData data = tankList[currentSelectedIndex];

        if (InventoryManager.Instance.HasTank(data.id))
        {
            // Đã có -> Trang bị
            InventoryManager.Instance.EquipTank(data.id);
        }
        else
        {
            // Chưa có -> Mua
            bool success = InventoryManager.Instance.BuyTank(data);
            if (success) UpdateRightPanel(currentSelectedIndex); // Cập nhật lại nút
        }
    }
}