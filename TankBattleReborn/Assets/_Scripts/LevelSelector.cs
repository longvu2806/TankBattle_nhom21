using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int levelID = 1; // Số thứ tự của Map (Sẽ chỉnh trong Inspector)

    // Hàm này sẽ gắn vào nút FIGHT
    public void SelectAndPlay()
    {
        // 1. Lưu lại là người chơi đã chọn Map số mấy
        PlayerPrefs.SetInt("SelectedMapID", levelID);

        // 2. Chuyển sang Scene GamePlay
        SceneManager.LoadScene("GamePlay");
    }
}