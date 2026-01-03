using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Các màn hình chính")]
    public GameObject loginPanel; // Kéo Login_Screen vào đây
    public GameObject menuPanel;  // Kéo Main_Menu vào đây
    public GameObject levelSelectionPanel; // Thêm cái này để chứa bảng chọn Map
    [Header("Các cửa sổ Popup (Kéo vào đây)")]
    public GameObject shopPanel;     // Kéo Shop_Popup vào đây
    public GameObject settingsPanel; // Kéo Settings_Popup vào đây (MỚI THÊM)

    // --- 1. LOGIC LOGIN ---
    public void OnClick_Launch()
    {
        loginPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    // --- 2. LOGIC MENU CHÍNH ---

    // Nút START: Sẽ chuyển sang màn chọn Map (Theo lộ trình mình vừa chốt)
    public void OnClick_StartGame()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false); // Tắt Menu chính

        if (levelSelectionPanel != null)
            levelSelectionPanel.SetActive(true); // Bật bảng chọn Map
    }

    // Nút SHOP (Hình xe đẩy)
    public void OnClick_OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true); // Hiện bảng Shop
        }
    }

    // Nút SETTING (Hình bánh răng)
    public void OnClick_OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Hiện bảng Cài Đặt
        }
    }
    // ... (Code cũ)

    // Hàm dùng cho nút X (Tắt hết các popup)
    public void CloseAllPopups()
    {
        // 1. Tắt Shop
        if (shopPanel != null) shopPanel.SetActive(false);

        // 2. Tắt Settings
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // 3. Tắt luôn cái nền tối (Nếu có dùng)
        // if (darkOverlay != null) darkOverlay.SetActive(false); 

        Debug.Log("Đã đóng tất cả Popup!");
    }
    // Hàm mở bảng chọn Map
    public void OpenLevelSelection()
    {
        menuPanel.SetActive(false);          // Tắt Menu chính (MenuPanel là biến cũ của bạn)
        levelSelectionPanel.SetActive(true); // Bật bảng chọn Map
    }

    // Hàm quay lại từ bảng chọn Map về Menu
    public void BackFromLevelSelection()
    {
        levelSelectionPanel.SetActive(false); // Tắt bảng chọn Map
        menuPanel.SetActive(true);            // Bật lại Menu chính
    }
    // Nút EXIT
    public void OnClick_Exit()
    {
        Debug.Log("Đã thoát game!");
        Application.Quit();
    }
}