using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Cài đặt Nhà máy")]
    public GameObject enemyPrefab;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    private void Start()
    {
        // Kiểm tra xem thời gian có đang chạy không
        if (Time.timeScale == 0)
        {
            Debug.LogError("LỖI: Thời gian đang bị dừng (TimeScale = 0)!");
        }

        // Gọi hàm đẻ trứng sau 2 giây, lặp lại mỗi spawnTime giây
        InvokeRepeating("SpawnEnemy", 2f, spawnTime);
    }

    void SpawnEnemy()
    {
        // 1. Kiểm tra xem Game có đang cho phép đẻ không
        if (GameManager.Instance.IsGameOver)
        {
            Debug.LogWarning("Không đẻ địch vì Game đang Over!");
            return;
        }

        Debug.Log(">>> ĐANG CỐ GẮNG ĐẺ ĐỊCH...");

        // 2. Chọn vị trí ngẫu nhiên
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        if (spawnPoint == null)
        {
            Debug.LogError("LỖI: Điểm sinh ra (Point) bị thiếu!");
            return;
        }

        // 3. Sinh ra xe địch
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log(">>> ĐÃ ĐẺ THÀNH CÔNG TẠI: " + spawnPoint.name);

        // 4. Báo cáo (Nếu bạn đã có hàm này bên GameManager thì bỏ comment ra)
        GameManager.Instance.RegisterEnemy();
    }
}