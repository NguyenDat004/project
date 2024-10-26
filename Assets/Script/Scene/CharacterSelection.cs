using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterSelection : MonoBehaviour
{
    public Image characterImage; // Hình ảnh nhân vật
    public Text characterNameText; // Tên nhân vật

    // Mảng chứa hình ảnh và tên các nhân vật
    public Sprite[] characters;
    public string[] characterNames;

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
    public void PlayGame()
    {
        // Chuyển đến màn hình chơi game
        
        SceneManager.LoadScene("MainGame"); // Chuyển sang màn chơi chính
    }
}
