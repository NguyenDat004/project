using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public Image characterImage;  // Hình ảnh nhân vật
    public Text characterNameText;  // Tên nhân vật
    public TMP_InputField playerNameInput;  // InputField để người chơi nhập tên
    public TMP_InputField roomInput;  // InputField để người chơi nhập phòng

    public Sprite[] characters;  // Mảng hình ảnh nhân vật
    public string[] characterNames;  // Mảng tên nhân vật

    private int currentIndex = 0;

    void Start()
    {
        UpdateCharacter();
    }

    // Khi nhấn nút "Next"
    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characters.Length)
        {
            currentIndex = 0; // Quay lại nhân vật đầu tiên
        }
        UpdateCharacter();
    }

    // Khi nhấn nút "Back"
    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = characters.Length - 1; // Chuyển đến nhân vật cuối cùng
        }
        UpdateCharacter();
    }

    // Cập nhật hình ảnh và tên nhân vật
    void UpdateCharacter()
    {
        characterImage.sprite = characters[currentIndex];
        characterNameText.text = characterNames[currentIndex];
    }

    // Khi nhấn nút "Play"
    public void PlayGame(){
    string playerName = playerNameInput.text;
    string roomName = roomInput.text;

    // Kiểm tra tên người chơi và tên phòng không rỗng
    if (string.IsNullOrEmpty(playerName))
    {
        Debug.LogWarning("Player name is required.");
        return;
    }

    if (string.IsNullOrEmpty(roomName))
    {
        Debug.LogWarning("Room name is required.");
        return;
    }

    // Lưu tên người chơi và tên phòng vào PlayerPrefs
    PlayerPrefs.SetString("PlayerName", playerName);  
    PlayerPrefs.SetString("RoomName", roomName);  // Lưu tên phòng

    // Lưu chỉ số nhân vật đã chọn vào PlayerPrefs
    PlayerPrefs.SetString("SelectedCharacter", characterNames[currentIndex]);
    PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);

    // Chuyển đến màn hình JoinRoom
    SceneManager.LoadScene("JoinRoom");
    }
    // Hàm để quay lại menu chính (tuỳ chọn)
    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Chạy lại thời gian
        SceneManager.LoadScene("MainMenu"); // Chuyển về Scene menu chính
    }
}
