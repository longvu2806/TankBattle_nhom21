using UnityEngine;
using UnityEngine.UI;
using TMPro; // Để dùng TextMeshPro
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("--- KẾT NỐI UI ---")]
    public Transform itemsParent;   // Kéo cái Content (trong ScrollView) vào đây
    public GameObject slotPrefab;   // Kéo Prefab ô đồ (Slot) vào đây

    [Header("--- PANEL CHI TIẾT (BÊN PHẢI) ---")]
    public Image detailIcon;
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailDesc;
    public Button equipButton;
    public TextMeshProUGUI equipButtonText; // Chữ trên nút (Trang Bị / Đang dùng)

    private ItemData selectedItem; // Món đồ đang được chọn

    void Start()
    {
        // Mặc định khi mở lên thì hiện tab Tất Cả (hoặc tab Xe Tăng)
        ShowCategory_All();

        // Ẩn panel chi tiết khi chưa chọn gì
        ClearDetailPanel();
    }

    // --- 1. CHỨC NĂNG LỌC DANH MỤC (Gắn vào các nút bên trái) ---

    public void ShowCategory_All()
    {
        RenderItems(InventoryManager.Instance.allGameItems); // Hiện hết
    }

    public void ShowCategory_Tanks()
    {
        FilterItemsByType(ItemType.Hull);
    }

    public void ShowCategory_Weapons()
    {
        FilterItemsByType(ItemType.Weapon);
    }

    void FilterItemsByType(ItemType typeToFilter)
    {
        List<ItemData> filteredList = new List<ItemData>();
        foreach (var item in InventoryManager.Instance.allGameItems)
        {
            if (item.itemType == typeToFilter)
            {
                filteredList.Add(item);
            }
        }
        RenderItems(filteredList);
    }

    // --- 2. HIỂN THỊ LƯỚI ITEMS ---

    void RenderItems(List<ItemData> itemsToRender)
    {
        // Xóa hết các icon cũ đi
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Tạo icon mới
        foreach (ItemData item in itemsToRender)
        {
            GameObject newSlot = Instantiate(slotPrefab, itemsParent);

            // Lấy script trên slot để set dữ liệu
            InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();
            if (slotScript != null)
            {
                slotScript.Setup(item, this); // Truyền 'this' để slot gọi ngược lại UI
            }
        }
    }

    // --- 3. CHỌN ITEM (Được gọi từ InventorySlot) ---

    public void SelectItem(ItemData item)
    {
        selectedItem = item;

        // Cập nhật UI bên phải
        detailIcon.sprite = item.icon;
        detailName.text = item.itemName;
        detailDesc.text = item.description; // + "\nChỉ số: ..."

        // Kiểm tra xem món này có đang được trang bị không?
        CheckEquipStatus();
    }

    void CheckEquipStatus()
    {
        bool isEquipped = false;

        if (selectedItem.itemType == ItemType.Hull)
        {
            isEquipped = (InventoryManager.Instance.equippedTankId == selectedItem.id);
        }
        else if (selectedItem.itemType == ItemType.Weapon)
        {
            isEquipped = (InventoryManager.Instance.equippedWeaponId == selectedItem.id);
        }

        if (isEquipped)
        {
            equipButtonText.text = "ĐANG DÙNG";
            equipButton.interactable = false; // Không bấm được nữa
        }
        else
        {
            equipButtonText.text = "TRANG BỊ";
            equipButton.interactable = true;
        }
    }

    void ClearDetailPanel()
    {
        detailIcon.sprite = null; // Hoặc hình mặc định
        detailName.text = "Chọn vật phẩm";
        detailDesc.text = "";
        equipButton.interactable = false;
    }

    // --- 4. NÚT TRANG BỊ (Gắn vào nút màu xanh lá) ---

    public void OnEquipButton_Clicked()
    {
        if (selectedItem == null) return;

        // Lưu vào InventoryManager
        if (selectedItem.itemType == ItemType.Hull)
        {
            InventoryManager.Instance.equippedTankId = selectedItem.id;
            Debug.Log("Đã trang bị xe: " + selectedItem.itemName);
        }
        else if (selectedItem.itemType == ItemType.Weapon)
        {
            InventoryManager.Instance.equippedWeaponId = selectedItem.id;
            Debug.Log("Đã trang bị súng: " + selectedItem.itemName);
        }

        // Cập nhật lại nút bấm
        CheckEquipStatus();
    }
}