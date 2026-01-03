using UnityEngine;
using TMPro;                // Bắt buộc để dùng TextMeshPro
using UnityEngine.SceneManagement; // Bắt buộc để chuyển màn (Load Scene)

public class ResultUI : MonoBehaviour
{
    [Header("--- BẢNG CHIẾN THẮNG (VICTORY) ---")]
    public GameObject victoryPanel;         // Cái Panel cha (chứa ảnh nền)
    public TextMeshProUGUI victoryTitleTxt; // Text: Danh hiệu (Sát thần...)
    public TextMeshProUGUI victoryScoreTxt; // Text: Điểm số
    public TextMeshProUGUI victoryGoldTxt;  // Text: Vàng
    public TextMeshProUGUI victoryKillsTxt; // Text: Số địch diệt
    public TextMeshProUGUI victoryTimeTxt;  // Text: Thời gian chơi

    [Header("--- BẢNG THUA CUỘC (GAME OVER) ---")]
    public GameObject losePanel;            // Cái Panel cha
    public TextMeshProUGUI loseScoreTxt;    // Text: Điểm số
    public TextMeshProUGUI loseGoldTxt;     // Text: Vàng
    public TextMeshProUGUI loseKillsTxt;    // Số địch diệt được
    public TextMeshProUGUI loseTimeTxt;     // Thời gian đã chơi

    // --- HÀM 1: HIỆN BẢNG THẮNG ---
    public void ShowVictory(int score, int gold, string title, int kills, float timePlayed)
    {
        // 1. Bật bảng lên
        victoryPanel.SetActive(true);

        // 2. Điền thông tin vào các ô trống
        victoryTitleTxt.text = title;
        victoryScoreTxt.text = score.ToString("#,##0"); // Format đẹp: 12,500
        victoryGoldTxt.text = "+" + gold.ToString();
        victoryKillsTxt.text = kills.ToString();

        // 3. Đổi giây sang phút:giây (Ví dụ 90s -> 01:30)
        string minutes = Mathf.Floor(timePlayed / 60).ToString("00");
        string seconds = (timePlayed % 60).ToString("00");
        victoryTimeTxt.text = string.Format("{0}:{1}", minutes, seconds);
    }

    // --- HÀM 2: HIỆN BẢNG THUA ---
    public void ShowLose(int score, int gold, int kills, float timePlayed)
    {
        losePanel.SetActive(true);

        // Điền thông tin
        loseScoreTxt.text = score.ToString("#,##0");
        loseGoldTxt.text = "+" + gold.ToString();
        loseKillsTxt.text = kills.ToString();

        // Xử lý thời gian (Giây -> Phút:Giây)
        string minutes = Mathf.Floor(timePlayed / 60).ToString("00");
        string seconds = (timePlayed % 60).ToString("00");
        loseTimeTxt.text = string.Format("{0}:{1}", minutes, seconds);
    }

    // --- CÁC HÀM CHO NÚT BẤM (BUTTONS) ---

    // Nút: Về màn hình chính (Home)
    public void OnClick_Home()
    {
        Time.timeScale = 1; // Quan trọng: Trả lại thời gian cho game chạy
        SceneManager.LoadScene("UIScene"); // Đảm bảo tên Scene Menu của bạn đúng là "UIScene" hoặc tên khác bạn đặt
    }

    // Nút: Chơi lại (Replay)
    public void OnClick_Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load lại màn hiện tại
    }

    // Nút: Chơi tiếp (Next Level)
    public void OnClick_NextLevel()
    {
        Time.timeScale = 1;
        // Tạm thời cho Replay, sau này bạn có nhiều Map thì sẽ code logic chuyển Map ở đây
        Debug.Log("Chuyển sang màn tiếp theo...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}