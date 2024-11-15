using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas; // Canvas màn hình kết thúc game

    void Start()
    {
        gameOverCanvas.SetActive(false); // Ẩn Canvas khi bắt đầu trò chơi
    }

    // Hàm này sẽ được gọi khi trò chơi kết thúc
    public void GameOver()
    {
        gameOverCanvas.SetActive(true); // Hiển thị màn hình kết thúc game
        Time.timeScale = 0f; // Dừng trò chơi (có thể không cần nếu bạn muốn chạy hoạt ảnh)
    }

    // Hàm để chơi lại trò chơi
    public void RetryGame()
    {
        Time.timeScale = 1f; // Chạy lại trò chơi
        SceneManager.LoadScene("MainMenu"); // Chuyển về Scene menu chính
    }

    public void QuitGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();

        // Nếu đang trong Unity Editor, hiển thị thông báo
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

}
