using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public Button btn; // Nút bấm của chính ô này

    private ItemData myData;
    private InventoryUI uiManager;

    public void Setup(ItemData data, InventoryUI ui)
    {
        myData = data;
        uiManager = ui;

        iconImage.sprite = data.icon;

        // Khi bấm vào ô này -> Gọi hàm SelectItem bên UI cha
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => uiManager.SelectItem(myData));
    }
}