using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 1. Singleton
    public static GameManager Instance;
    private float startTime;
    public bool IsGameOver = false;

    // --- PHẦN CỦA BẠN B: Biến đếm số địch ---
    [Header("Quản lý Chiến Thắng")]
    public int enemyCount = 0;

    [Header("Điểm số")]
    public int score = 0; // Biến lưu điểm (100 điểm = 100 vàng)
    // ban A moi them
    [Header("UI GamePlay (Tactical Badge)")]
    public TextMeshProUGUI enemyCountText; // Kéo Txt_Value vào đây
    public Animator scoreAnimator;         // Kéo Txt_Value (có Animator) vào đây
    public AudioSource audioSource;        // Kéo AudioSource vào đây
    public AudioClip popSound;             // Kéo file âm thanh vào đây
    [Header("UI Kết Thúc (Win/Lose)")]
    public ResultUI resultUI; 
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
        enemyCount++; // Tăng số lượng lên 1
        UpdateEnemyUI(false); // Cập nhật UI ngay (ví dụ từ 0 -> 1)
        Debug.Log("Địch báo danh! Tổng: " + enemyCount);
    }

    // --- LOGIC CỦA BẠN B: Đếm số địch có sẵn lúc bắt đầu ---
    void Start()
    {
        enemyCount = 0;
        UpdateEnemyUI(false);
        startTime = Time.time;
    }

    // --- LOGIC CỦA BẠN B: Xử lý khi địch chết ---
    public void EnemyDied()
    {
        enemyCount--; // Trừ đi 1 tên
        Debug.Log("Địch chết! Còn lại: " + enemyCount);

        score += 100; // Cộng 100 điểm mỗi khi giết 1 tên
        // [MỚI] Cập nhật UI với hiệu ứng Giật nảy + Âm thanh
        UpdateEnemyUI(true);

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

    // [MỚI] HÀM RIÊNG ĐỂ XỬ LÝ UI GIẬT NẢY
    // ================================================================
    void UpdateEnemyUI(bool playEffect)
    {
        // 1. Đổi con số (Format "00" để số 5 hiện là 05)
        if (enemyCountText != null)
        {
            enemyCountText.text = enemyCount.ToString("00");
        }

        // 2. Chạy hiệu ứng (Chỉ chạy khi playEffect = true)
        if (playEffect)
        {
            if (scoreAnimator != null) scoreAnimator.SetTrigger("Pop");

            if (audioSource != null && popSound != null) audioSource.PlayOneShot(popSound);
        }
    }
    // ================================================================
    // --- HÀM XỬ LÝ GAME OVER (THUA) ---
    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Time.timeScale = 0;

        // 1. Tính toán số liệu (Dù thua vẫn phải tính để hiện báo cáo)
        float playTime = Time.time - startTime;

        // Thua thì chỉ nhận được Vàng = Điểm (Không có thưởng nóng)
        int totalGold = score;

        // 2. Cộng tiền an ủi
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddCoin(totalGold);

        // 3. Hiện bảng Lose
        if (resultUI != null)
        {
            // Lấy số kill hiện tại (lấy biến enemyCount ban đầu trừ đi biến hiện tại, hoặc biến đếm kill riêng)
            // Tạm thời để ví dụ là 5
            int currentKills = 5;

            // Gọi hàm ShowLose mới
            resultUI.ShowLose(score, totalGold, currentKills, playTime);
        }
    }

    // --- HÀM XỬ LÝ VICTORY (THẮNG) ---
    public void Victory()
    {
        if (IsGameOver) return; // Nếu đã thắng/thua rồi thì thôi
        IsGameOver = true;

        Debug.Log("VICTORY! Bạn đã thắng.");
        Time.timeScale = 0;
        int bonusReward = 500;
        int totalGold = score + bonusReward;
        float playTime = Time.time - startTime;
        string titleReceived = "LÍNH MỚI";
            if (playTime < 60) titleReceived = "SÁT THẦN TỐC ĐỘ";
        // (Bạn có thể thêm logic danh hiệu phức tạp hơn ở đây)
        // ================================================================
        // [MỚI] TRẢ LƯƠNG CHIẾN THẮNG
        // Logic: Nhận toàn bộ điểm Score + Thưởng nóng 500 vàng
        // ================================================================
        if (InventoryManager.Instance != null)
        {

            InventoryManager.Instance.AddCoin(totalGold);
            Debug.Log($"Chiến thắng! Tổng tiền nhận: {totalGold} (Score: {score} + Thưởng: {bonusReward})");
        }
        // ================================================================

        // Gọi UI hiện bảng Victory
        if (resultUI != null)
        {
            // Các biến kills, playTime phải được tính hoặc truyền vào
            // Ví dụ lấy tạm kills = 15
            resultUI.ShowVictory(score, totalGold, titleReceived, 15, playTime);
        }
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}