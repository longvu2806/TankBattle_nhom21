using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện quản lý Scene

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject loginPanel; // Kéo Login_Screen vào đây
    public GameObject menuPanel;  // Kéo Main_Menu vào đây
    public GameObject shopPanel;  // (Để dành làm sau)

    // --- LOGIC LOGIN ---
    public void OnClick_Launch()
    {
        // Khi bấm nút LAUNCH ở màn hình Login
        // Tắt Login, Bật Menu
        loginPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    // --- LOGIC MENU ---
    public void OnClick_StartGame()
    {
        // Chuyển sang màn chơi chính
        SceneManager.LoadScene("GamePlay");
    }

    public void OnClick_Exit()
    {
        Debug.Log("Thoát Game");
        Application.Quit();
    }
}