using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinRoomManager : MonoBehaviour
{
    public Image characterImage;  // Image component to display the character
    public Text characterNameText;  // Text component to display the character name
    public Text playerNameText;  // Text component to display the player's name
    public TMP_Text roomNameText;  // TextMeshPro để hiển thị tên phòng

    public Sprite[] characters;  // The array of character images
    public string[] characterNames;  // The array of character names

    void Start()
    {
        // Kiểm tra nếu PlayerPrefs có lưu tên phòng
        if (PlayerPrefs.HasKey("RoomName"))
        {
            string roomName = PlayerPrefs.GetString("RoomName");
            roomNameText.text = "Room: " + roomName;  // Hiển thị tên phòng
        }
        else
        {
            roomNameText.text = "No Room Selected";  // Nếu không có tên phòng, hiển thị thông báo
        }
        // Get the selected character index from PlayerPrefs
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        // Display the character's image and name
        if (selectedIndex >= 0 && selectedIndex < characters.Length)
        {
            characterImage.sprite = characters[selectedIndex];
            characterNameText.text = characterNames[selectedIndex];
        }
        else
        {
            Debug.LogError("Selected character index is out of bounds.");
        }

        // Get the player's name from PlayerPrefs and display it
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        playerNameText.text = playerName;  // Hiển thị tên người chơi
    }

    // Hàm để quay lại menu chính
    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Đảm bảo thời gian không bị tạm dừng
        SceneManager.LoadScene("MainMenu"); // Chuyển về Scene menu chính
    }

    // Hàm khi nhấn nút "Play" để chuyển sang màn chơi chính
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame"); // Chuyển đến Scene "MainGame"
    }
}
