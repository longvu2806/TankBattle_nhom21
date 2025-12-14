using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 1. Singleton
    public static GameManager Instance;

    public bool IsGameOver = false;

    // --- PHẦN MỚI 1: Biến đếm số địch ---
    [Header("Quản lý Chiến Thắng")]
    public int enemyCount = 0;
    // ------------------------------------
    [Header("Điểm số")]
    public int score = 0; // Biến lưu điểm
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
    public void RegisterEnemy()
    {
        enemyCount++; // Tăng số lượng địch lên 1
        Debug.Log("Địch mới xuất hiện! Tổng: " + enemyCount);
    }

    // --- PHẦN MỚI 2: Đếm số địch lúc bắt đầu ---
    void Start()
    {
        // Tìm tất cả GameObject có Tag là "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;

        Debug.Log("Bắt đầu game với: " + enemyCount + " kẻ địch.");
    }
    // -------------------------------------------

    // Hàm xử lý Game Over (Thua)
    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Debug.Log("Game Over!");

        Time.timeScale = 0; // Dừng game

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPanel("GameOver");
        }
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // --- PHẦN MỚI 3: Xử lý khi địch chết ---
    public void EnemyDied()
    {
        enemyCount--; // Trừ đi 1 tên
        Debug.Log("Địch chết! Còn lại: " + enemyCount);
        score += 100; // Cộng 100 điểm mỗi khi giết 1 tên
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreUI(score);
        }
        // ---------------------
        // Nếu hết địch thì Thắng
        if (enemyCount <= 0)
        {
            Victory();
        }
    }
    // ---------------------------------------

    // Hàm xử lý Victory (Thắng) - Đã cập nhật
    public void Victory()
    {
        if (IsGameOver) return; // Nếu đã thắng/thua rồi thì thôi
        IsGameOver = true;

        Debug.Log("VICTORY! Bạn đã thắng.");
        Time.timeScale = 0;

        // Gọi UI hiện bảng Victory (Nhớ bỏ comment sau khi update UIManager)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPanel("Victory");
        }
    }
}