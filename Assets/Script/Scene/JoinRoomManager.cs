using UnityEngine;
using UnityEngine.UI;

public class JoinRoomManager : MonoBehaviour
{
    public Text characterNameText; // Tham chiếu đến Text trong Panel
    public Image characterImage; // Tham chiếu đến Image trong Panel
    public Sprite[] characters; // Mảng chứa hình ảnh nhân vật

    void Start()
    {
        // Lấy tên nhân vật đã chọn từ PlayerPrefs
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Chưa chọn nhân vật");
        characterNameText.text = "Nhân vật của bạn: " + selectedCharacter; // Hiển thị tên nhân vật
        
        // Lấy chỉ số hình ảnh của nhân vật đã chọn từ PlayerPrefs
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        
        // Hiển thị hình ảnh của nhân vật đã chọn
        characterImage.sprite = characters[selectedCharacterIndex];
    }
}
