using System.Collections;
using TMPro;
using UnityEngine;

public class thongbaochet : MonoBehaviour
{
    private float fadeDuration = 3.0f; // Thời gian mờ dần (3 giây)
    private TMP_Text textdie;

    private void Start()
    {
        // Lấy tham chiếu đến TMP_Text và đặt màu bắt đầu là đỏ với alpha là 1
        textdie = GetComponent<TMP_Text>();
        if (textdie != null)
        {
            textdie.color = new Color(1, 0, 0, 1); // Đỏ với alpha = 1
            StartCoroutine(FadeOutText());
        }
        else
        {
            Debug.LogError("TMP_Text component not found!");
        }
    }

    private IEnumerator FadeOutText()
    {
        float startAlpha = textdie.color.a;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        // Giảm dần alpha (độ trong suốt) của văn bản trong thời gian fadeDuration
        while (progress < 1.0f)
        {
            Color tmpColor = textdie.color;
            tmpColor.a = Mathf.Lerp(startAlpha, 0, progress);
            textdie.color = tmpColor;

            progress += rate * Time.deltaTime;
            yield return null;
        }

        // Đảm bảo alpha bằng 0 sau khi kết thúc mờ dần
        Color finalColor = textdie.color;
        finalColor.a = 0;
        textdie.color = finalColor;

        // Hủy đối tượng sau khi mờ dần (tùy chọn)
        Destroy(gameObject);
    }
}
