using UnityEngine;
using System.IO; // Thư viện đọc ghi file

public static class SaveSystem
{
    // Đường dẫn file save (trên máy tính hay điện thoại đều chạy được)
    private static string path = Application.persistentDataPath + "/playerdata.json";

    public static void Save(PlayerData data)
    {
        // 1. Biến object thành chuỗi JSON
        string json = JsonUtility.ToJson(data, true); // true để viết đẹp dễ đọc

        // 2. Ghi chuỗi đó xuống file
        File.WriteAllText(path, json);

        Debug.Log("Đã lưu game tại: " + path);
    }

    public static PlayerData Load()
    {
        if (File.Exists(path))
        {
            // 1. Đọc chuỗi từ file lên
            string json = File.ReadAllText(path);

            // 2. Biến chuỗi JSON ngược lại thành object
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            // Nếu chưa có file save (chơi lần đầu) -> Tạo mới
            Debug.Log("Không tìm thấy file save. Tạo mới.");
            return new PlayerData();
        }
    }
}