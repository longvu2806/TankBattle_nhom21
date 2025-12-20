using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 1. Singleton
    public static GameManager Instance;

    public bool IsGameOver = false;

    // --- PHẦN CỦA BẠN B: Biến đếm số địch ---
    [Header("Quản lý Chiến Thắng")]
    public int enemyCount = 0;

    [Header("Điểm số")]
    public int score = 0; // Biến lưu điểm (100 điểm = 100 vàng)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- LOGIC CỦA BẠN B: Đăng ký địch sinh ra từ Spawner ---
    public void RegisterEnemy()
    {
        enemyCount++; // Tăng số lượng địch lên 1
        Debug.Log("Địch mới xuất hiện! Tổng: " + enemyCount);
    }

    // --- LOGIC CỦA BẠN B: Đếm số địch có sẵn lúc bắt đầu ---
    void Start()
    {
        // Tìm tất cả GameObject có Tag là "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;

        Debug.Log("Bắt đầu game với: " + enemyCount + " kẻ địch.");
    }

    // --- LOGIC CỦA BẠN B: Xử lý khi địch chết ---
    public void EnemyDied()
    {
        enemyCount--; // Trừ đi 1 tên
        Debug.Log("Địch chết! Còn lại: " + enemyCount);

        score += 100; // Cộng 100 điểm mỗi khi giết 1 tên

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
        IsGameOver = true;
        Debug.Log("Game Over!");

        Time.timeScale = 0; // Dừng game

        // ================================================================
        // [MỚI] TRẢ LƯƠNG AN ỦI KHI THUA
        // Logic: Bắn được bao nhiêu điểm thì nhận bấy nhiêu tiền
        // ================================================================
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddCoin(score);
            Debug.Log($"Thua cuộc! Nhận tiền an ủi: {score}");
        }
        // ================================================================

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPanel("GameOver");
        }
    }

    // --- HÀM XỬ LÝ VICTORY (THẮNG) ---
    public void Victory()
    {
        if (IsGameOver) return; // Nếu đã thắng/thua rồi thì thôi
        IsGameOver = true;

        Debug.Log("VICTORY! Bạn đã thắng.");
        Time.timeScale = 0;

        // ================================================================
        // [MỚI] TRẢ LƯƠNG CHIẾN THẮNG
        // Logic: Nhận toàn bộ điểm Score + Thưởng nóng 500 vàng
        // ================================================================
        if (InventoryManager.Instance != null)
        {
            int bonusReward = 500;
            int totalGold = score + bonusReward;

            InventoryManager.Instance.AddCoin(totalGold);
            Debug.Log($"Chiến thắng! Tổng tiền nhận: {totalGold} (Score: {score} + Thưởng: {bonusReward})");
        }
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
}