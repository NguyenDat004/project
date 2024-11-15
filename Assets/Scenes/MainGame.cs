using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MainGame : MonoBehaviour
{
    public GameObject rolePanel; // Tham chiếu đến Panel UI trong Inspector
    public float panelHideDelay = 5f; // Thời gian ẩn panel sau khi game bắt đầu (tính bằng giây)

    private void Start()
    {
        // Kiểm tra xem người chơi có phải là host không
        if (NetworkManager.Singleton.IsHost)
        {
            // Nếu là host, ẩn ngay lập tức
            rolePanel.SetActive(false);
        }
        else
        {
            // Nếu là client, hiển thị panel trong một khoảng thời gian ngắn rồi ẩn đi
            rolePanel.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(panelHideDelay)); // Chạy coroutine để ẩn panel sau thời gian delay
        }
    }

    // Coroutine để ẩn panel sau một khoảng thời gian
    private IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rolePanel.SetActive(false); // Ẩn panel sau delay
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Resume time if it was paused
        SceneManager.LoadScene("SelectCharacter"); // Load the main menu scene
    }
}
