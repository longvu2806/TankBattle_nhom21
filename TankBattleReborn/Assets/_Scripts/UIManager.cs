using UnityEngine;
using TMPro; // Thư viện hiển thị chữ nét căng (TextMeshPro)

public class UIManager : MonoBehaviour
{
    // Singleton: Để gọi UIManager.Instance ở bất cứ đâu
    public static UIManager Instance { get; private set; }

    [Header("Các Panel Giao Diện (Bạn A sẽ kéo vào đây)")]
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject shopPanel;
    public GameObject victoryPanel;


    [Header("Các dòng chữ thông số (HUD)")]
    public TextMeshProUGUI healthText; // Hiển thị máu
    public TextMeshProUGUI scoreText;  // Hiển thị điểm (sau này làm)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1; // Đảm bảo thời gian chạy bình thường khi vào game
        // Mặc định khi chạy game thì hiện Menu, tắt mấy cái kia
        //ShowPanel("Menu");
    }

    // Hàm chức năng: Bật panel này, tắt panel kia
    public void ShowPanel(string panelName)
    {
        // 1. Tắt hết đi đã
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (gamePanel) gamePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (shopPanel) shopPanel.SetActive(false);


        // 2. Bật cái cần thiết lên
        switch (panelName)
        {
            case "Menu":
                if (mainMenuPanel) mainMenuPanel.SetActive(true);
                Time.timeScale = 0; // Dừng thời gian (Game chưa chạy)
                break;

            case "Game":
                if (gamePanel) gamePanel.SetActive(true);
                Time.timeScale = 1; // Chạy game bình thường
                break;

            case "Shop":
                if (shopPanel) shopPanel.SetActive(true);
                break;

            case "GameOver":
                if (gameOverPanel) gameOverPanel.SetActive(true);
                // Không dừng thời gian ở đây để còn xem hiệu ứng nổ
                break;
            case "Victory":
                if (victoryPanel) victoryPanel.SetActive(true);
                break;
        }
    }


    // Hàm cập nhật máu trên màn hình
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            // Hiển thị dạng "HP: 80 / 100"
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        }
    }
    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
