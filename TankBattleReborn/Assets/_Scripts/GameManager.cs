using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để reset game

public class GameManager : MonoBehaviour
{
    // Singleton: Giúp gọi GameManager ở bất cứ đâu
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Hàm xử lý khi thua (TankController sẽ gọi hàm này)
    public void TriggerGameOver()
    {
        Debug.Log("--- GAME OVER! ---");
        // Đợi 2 giây rồi chơi lại
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        // Load lại màn chơi hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}