using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc có dòng này để chuyển cảnh

public class MainMenuController : MonoBehaviour
{
    // Hàm này dùng cho nút START
    public void PlayGame()
    {
        // Khi bấm Start, máy sẽ tải Scene số 1 trong danh sách (Scene Game)
        SceneManager.LoadScene(1);
    }

    // Hàm này dùng cho nút EXIT
    public void QuitGame()
    {
        Debug.Log("Đã thoát game!"); // Hiện thông báo kiểm tra
        Application.Quit();
    }
}