using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

 public void Room()
{
    Debug.Log("Đang chuyển đến màn hình chọn nhân vật...");
    SceneManager.LoadScene("SelectCharacter"); // Hiển thị màn chọn nhân vật
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
