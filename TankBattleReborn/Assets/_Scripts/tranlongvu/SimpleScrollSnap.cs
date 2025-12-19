using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleScrollSnap : MonoBehaviour, IEndDragHandler
{
    public ScrollRect scrollRect; // Kéo cái Scroll View vào đây
    public float snapSpeed = 10f; // Tốc độ trượt về đích

    private float[] points;
    private int pageCount;
    private float stepSize;
    private bool isSnapping = false;
    private float targetPosition;

    void Start()
    {
        // Tự động tính toán vị trí các trang (0.0 đến 1.0)
        pageCount = scrollRect.content.childCount;
        if (pageCount > 1)
        {
            points = new float[pageCount];
            stepSize = 1f / (pageCount - 1);
            for (int i = 0; i < pageCount; i++)
            {
                points[i] = i * stepSize;
            }
        }
    }

    // Khi người chơi thả tay ra sau khi kéo
    public void OnEndDrag(PointerEventData eventData)
    {
        float currentPos = scrollRect.horizontalNormalizedPosition;
        float minDistance = Mathf.Infinity;

        // Tìm xem đang gần trang nào nhất
        foreach (float p in points)
        {
            float dist = Mathf.Abs(p - currentPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                targetPosition = p;
            }
        }
        isSnapping = true;
    }

    void Update()
    {
        if (isSnapping && pageCount > 1)
        {
            // Lướt từ từ về trang mục tiêu
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(
                scrollRect.horizontalNormalizedPosition,
                targetPosition,
                snapSpeed * Time.deltaTime
            );

            // Nếu đã đến rất gần thì dừng lại
            if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPosition) < 0.001f)
            {
                scrollRect.horizontalNormalizedPosition = targetPosition;
                isSnapping = false;
            }
        }
    }
}