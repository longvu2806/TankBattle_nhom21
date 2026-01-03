using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject[] mapPrefabs; // Danh sách chứa các Prefab Map 1, Map 2...

    void Start()
    {
        // 1. Đọc xem người chơi đã chọn Map số mấy (Mặc định là 1)
        int selectedID = PlayerPrefs.GetInt("SelectedMapID", 1);

        // 2. Kiểm tra xem có Map đó trong kho không
        // (Vì mảng bắt đầu từ 0, nên Map 1 sẽ nằm ở vị trí 0 -> trừ đi 1)
        int index = selectedID - 1;

        if (index >= 0 && index < mapPrefabs.Length)
        {
            // 3. Sinh ra Map đó tại vị trí (0,0,0)
            Instantiate(mapPrefabs[index], Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Chưa bỏ Map vào kho hoặc sai ID!");
        }
    }
}