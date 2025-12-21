using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 1. Singleton
    public static GameManager Instance;

    // --- [MỚI] DATA SYSTEM ---
    [Header("Dữ liệu Người chơi (JSON)")]
    public PlayerData playerData; // Biến này chứa Tiền, Level, Danh sách xe
    // -------------------------

    public bool IsGameOver = false;

    // --- PHẦN CỦA BẠN B: Biến đếm số địch ---
    [Header("Quản lý Chiến Thắng")]
    public int enemyCount = 0;

    [Header("Điểm số")]
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // [QUAN TRỌNG] Giữ GameManager sống qua các màn
        }
        else
        {
            Destroy(gameObject);
            return; // Thoát luôn để tránh lỗi logic bên dưới
        }

        // --- [MỚI] LOAD DỮ LIỆU TỪ FILE JSON ---
        playerData = SaveSystem.Load();
        Debug.Log($"Game khởi động. Tài sản hiện có: {playerData.gold} Gold");
    }

    // --- LOGIC CỦA BẠN B: Đăng ký địch sinh ra từ Spawner ---
    public void RegisterEnemy()
    {
        enemyCount++;
        // Debug.Log("Địch mới xuất hiện! Tổng: " + enemyCount);
    }

    // --- LOGIC CỦA BẠN B: Đếm số địch có sẵn lúc bắt đầu ---
    void Start()
    {
        // Reset trạng thái game
        IsGameOver = false;
        Time.timeScale = 1;
        score = 0; // Reset điểm về 0 khi chơi mới

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;
        // Debug.Log("Bắt đầu game với: " + enemyCount + " kẻ địch.");
    }

    // --- LOGIC CỦA BẠN B: Xử lý khi địch chết ---
    public void EnemyDied()
    {
        enemyCount--;
        // Debug.Log("Địch chết! Còn lại: " + enemyCount);

        score += 100; // Cộng điểm (tạm thời chưa lưu, chết/thắng mới đổi thành tiền)

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreUI(score);
        }

        // Nếu hết địch thì Thắng
        if (enemyCount <= 0)
        {
            Victory();
        }
    }

    // --- HÀM XỬ LÝ GAME OVER (THUA) ---
    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = false;
        Debug.Log("Game Over!");

        Time.timeScale = 0; // Dừng game

        // ================================================================
        // [NÂNG CẤP] LƯU DỮ LIỆU XUỐNG JSON
        // ================================================================
        // Cộng tiền an ủi vào kho dữ liệu chính
        playerData.gold += score;

        // GỌI LỆNH LƯU NGAY LẬP TỨC
        SaveGame();

        Debug.Log($"Thua cuộc! Đã lưu thêm {score} vàng vào ví. Tổng: {playerData.gold}");
        // ================================================================

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPanel("GameOver");
        }
    }

    // --- HÀM XỬ LÝ VICTORY (THẮNG) ---
    public void Victory()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        Debug.Log("VICTORY! Bạn đã thắng.");
        Time.timeScale = 0;

        // ================================================================
        // [NÂNG CẤP] LƯU DỮ LIỆU XUỐNG JSON
        // ================================================================
        int bonusReward = 500;
        int totalGold = score + bonusReward;

        // Cộng tiền vào Data
        playerData.gold += totalGold;

        // GỌI LỆNH LƯU NGAY LẬP TỨC
        SaveGame();

        Debug.Log($"Chiến thắng! Đã lưu {totalGold} vàng. Tổng ví: {playerData.gold}");
        // ================================================================

        // Gọi UI hiện bảng Victory
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPanel("Victory");
        }
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- [MỚI] HÀM TIỆN ÍCH CHO SHOP & SAVE ---

    // Gọi hàm này để lưu game bất cứ lúc nào
    public void SaveGame()
    {
        SaveSystem.Save(playerData);
    }

    // Tự động lưu khi tắt game (để chắc ăn)
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}