﻿using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class JoinRoomManager : MonoBehaviour
{
    // UI component hiển thị hình ảnh nhân vật đã chọn
    public Image characterImage;
    // UI component hiển thị tên nhân vật đã chọn
    public Text characterNameText;
    // UI component hiển thị tên người chơi
    public Text playerNameText;
    // TextMeshPro để hiển thị tên phòng
    public TMP_Text roomNameText;

    // Mảng chứa hình ảnh các nhân vật
    public Sprite[] characters;
    // Mảng chứa tên của các nhân vật
    public string[] characterNames;

    // Các nút để bắt đầu host và client
    [SerializeField]
    private Button hostButton;
    [SerializeField]
    private Button clientButton;

    void Start()
    {
        // Inside the Start method or a custom initialization method
        hostButton.onClick.AddListener(() =>
        {
            Debug.Log("Host button pressed in Scene 1.");
            // Start a coroutine to load the scene and then start the host
            StartCoroutine(LoadSceneAndStartHost("MainGame"));
        });

        clientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client button pressed in Scene 1.");
            // Start a coroutine to load the scene and then start the client
            StartCoroutine(LoadSceneAndStartClient("MainGame"));
        });


        // Kiểm tra xem PlayerPrefs có lưu tên phòng không
        if (PlayerPrefs.HasKey("RoomName"))
        {
            // Lấy tên phòng từ PlayerPrefs và hiển thị
            string roomName = PlayerPrefs.GetString("RoomName");
            roomNameText.text = "Room: " + roomName;
        }
        else
        {
            // Nếu không có tên phòng, hiển thị thông báo mặc định
            roomNameText.text = "No Room Selected";
        }

        // Lấy chỉ số nhân vật đã chọn từ PlayerPrefs
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        // Kiểm tra nếu chỉ số hợp lệ và hiển thị hình ảnh, tên nhân vật
        if (selectedIndex >= 0 && selectedIndex < characters.Length)
        {
            characterImage.sprite = characters[selectedIndex];
            characterNameText.text = characterNames[selectedIndex];
        }
        else
        {
            Debug.LogError("Selected character index is out of bounds.");
        }

        // Lấy tên người chơi từ PlayerPrefs và hiển thị
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        playerNameText.text = playerName;
    }

    // Hàm để quay lại menu chính khi bấm nút "Return"
    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Đảm bảo thời gian không bị tạm dừng khi quay lại menu
        SceneManager.LoadScene("MainMenu"); // Chuyển về scene menu chính
    }// Hàm khi nhấn nút "Play" để chuyển sang màn chơi chính

    private IEnumerator LoadSceneAndStartHost(string sceneName)
    {
        NetworkManager.Singleton.StartHost();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // Wait until the scene has loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // Start the host after the scene has loaded
        
        Debug.Log("Host started in MainGame scene.");
    }

    private IEnumerator LoadSceneAndStartClient(string sceneName)
    {
        NetworkManager.Singleton.StartClient();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // Wait until the scene has loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // Start the client after the scene has loaded
        
        Debug.Log("Client started in MainGame scene.");
    }

    public void PlayGame()
    {
        //hostButton.onClick.AddListener(() =>
        //{
        //    SceneManager.LoadScene("MainGame"); // Chuyển đến Scene "MainGame"
        //    NetworkManager.Singleton.StartHost();
        //});
        //clientButton.onClick.AddListener(() =>
        //{
        //    SceneManager.LoadScene("MainGame"); // Chuyển đến Scene "MainGame"
        //    NetworkManager.Singleton.StartClient();
        //});
    }
}