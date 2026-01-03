using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCardUI : MonoBehaviour
{
    [Header("--- THÀNH PHẦN GIAO DIỆN ---")]
    public Image iconImage;         // Kéo ảnh Icon xe tăng vào
    public Image rarityGlow;        // Kéo cái vòng sáng nền (Rarity_Glow) vào
    public Image selectionRing;     // Kéo cái vòng xoay chọn (Selection_Ring) vào
    public TextMeshProUGUI priceText; // Kéo chữ giá tiền vào

    [Header("--- CÀI ĐẶT MÀU SẮC ---")]
    public Color commonColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);   // Xám
    public Color rareColor = new Color(0, 0.5f, 1f, 0.6f);          // Xanh dương
    public Color epicColor = new Color(0.7f, 0, 1f, 0.6f);          // Tím
    public Color legendaryColor = new Color(1f, 0.8f, 0, 0.7f);     // Vàng Cam

    // Biến lưu trữ trạng thái
    private bool isSelected = false;
    private int myIndex;
    private ShopController shopController;

    void Start()
    {
        // Mặc định tắt vòng xoay khi mới sinh ra
        if (selectionRing != null)
            selectionRing.gameObject.SetActive(false);
    }

    void Update()
    {
        // LOGIC XOAY VÒNG: Chỉ xoay khi đang được chọn
        if (isSelected && selectionRing != null)
        {
            // Xoay -150 độ mỗi giây (xoay phải)
            selectionRing.transform.Rotate(0, 0, -150 * Time.deltaTime);
        }
    }

    // --- [ĐÃ SỬA] HÀM 1: CÀI ĐẶT DỮ LIỆU TỪ ITEM DATA ---
    // Bây giờ nhận vào (int index, ShopController, ItemData)
    public void Setup(int index, ShopController controller, ItemData data)
    {
        myIndex = index;
        shopController = controller;

        // 1. Điền giá và ảnh
        if (priceText) priceText.text = "$ " + data.price;
        if (iconImage) iconImage.sprite = data.icon;

        // 2. Chỉnh màu độ hiếm
        // (Ép kiểu Enum Rarity sang số nguyên int để dùng switch cũ của bạn)
        int rarityIndex = (int)data.rarity;

        Color finalColor = commonColor; // Mặc định là Common (0)
        switch (rarityIndex)
        {
            case 1: finalColor = rareColor; break;      // Rare
            case 2: finalColor = epicColor; break;      // Epic
            case 3: finalColor = legendaryColor; break; // Legendary
        }

        if (rarityGlow) rarityGlow.color = finalColor;

        // 3. Đặt tên GameObject cho dễ quản lý
        this.gameObject.name = "Card_" + data.itemName;
    }

    // --- HÀM 2: XỬ LÝ KHI NGƯỜI CHƠI BẤM VÀO THẺ ---
    public void OnClick_SelectCard()
    {
        if (shopController != null)
        {
            shopController.OnCardSelected(myIndex);
        }
    }

    // --- HÀM 3: BẬT/TẮT TRẠNG THÁI CHỌN ---
    public void SetSelectState(bool selected)
    {
        isSelected = selected;

        // 1. Bật/Tắt vòng xoay
        if (selectionRing != null)
            selectionRing.gameObject.SetActive(selected);

        // 2. Hiệu ứng Phóng to / Thu nhỏ
        if (selected)
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            if (selectionRing != null) selectionRing.transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}