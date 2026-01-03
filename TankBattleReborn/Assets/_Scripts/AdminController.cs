using UnityEngine;
using TMPro; // Nhớ dùng thư viện này nếu dùng TextMeshPro

public class AdminController : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField inputName; // Ô nhập tên vật phẩm
    public TMP_InputField inputPrice; // Ô nhập giá
    public TextMeshProUGUI txtNotification; // Dòng chữ thông báo kết quả

    // Hàm này sẽ gắn vào nút "Thêm" (Add Button)
    public void OnAddButtonClick()
    {
        // 1. Kiểm tra xem người dùng có nhập thiếu không
        if (string.IsNullOrEmpty(inputName.text) || string.IsNullOrEmpty(inputPrice.text))
        {
            txtNotification.text = "Lỗi: Vui lòng nhập đầy đủ thông tin!";
            txtNotification.color = Color.red;
            return; // Dừng lại không làm tiếp
        }

        // 2. Logic GIẢ (Fake Logic): Chỉ in ra Console và hiện thông báo
        Debug.Log("Admin đã thêm: " + inputName.text + " với giá: " + inputPrice.text);

        // 3. Hiện thông báo thành công cho người xem thấy
        txtNotification.text = "Thêm vật phẩm thành công! (Dữ liệu giả)";
        txtNotification.color = Color.green;

        // 4. Xóa trắng ô nhập để nhập cái tiếp theo (cho trải nghiệm tốt)
        inputName.text = "";
        inputPrice.text = "";
    }
}