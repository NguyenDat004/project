using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class StartGame : MonoBehaviour
{
    public Button startButton; // Tham chiếu đến nút trong Inspector
    public Button startClient; // Tham chiếu đến nút trong Inspector
    private void Start()
    {
        // Kiểm tra xem người chơi có phải là host không
        if (NetworkManager.Singleton.IsHost)
        {
            // Hiển thị nút nếu người chơi là host
            startButton.gameObject.SetActive(true);
            startClient.gameObject.SetActive(false);
        }
        else
        {
            // Ẩn nút nếu người chơi là client
            startButton.gameObject.SetActive(false);
            startClient.gameObject.SetActive(true);
        }
    }
    
    public void NextScene()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Resume time if it was paused
        SceneManager.LoadScene("SelectCharacter"); // Load the main menu scene
    }
}