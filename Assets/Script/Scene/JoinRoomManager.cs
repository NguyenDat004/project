using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Unity.Netcode.Transports.UTP;

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

    private string ipAddress; // Địa chỉ IP sẽ được lấy tự động\
    private UnityTransport transport;

    public TMP_InputField IPAddressHost;

    public TMP_Text IPAddressName;
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
            ipAddress = IPAddressHost.text;
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
        string ipAddress = GetLocalIPAddress(); // Hàm lấy địa chỉ IP
        IPAddressName.text= ipAddress;
        PlayerPrefs.SetString("HostIPAddress", ipAddress);
        PlayerPrefs.Save();
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
        string hostIp = PlayerPrefs.GetString("HostIPAddress", ipAddress); // Lấy IP từ PlayerPrefs
        Debug.Log(ipAddress);
        transport.ConnectionData.Address = ipAddress; // Thiết lập địa chỉ IP cho client
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // Wait until the scene has loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // Start the client after the scene has loaded

        Debug.Log("Client started in MainGame scene.");
    }


    /* Gets the Ip Address of your connected network and
shows on the screen in order to let other players join
by inputing that Ip in the input field */
    // ONLY FOR HOST SIDE 
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
                Debug.Log("IP của host: " + ipAddress);


                //PrintOpenPorts();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }


    /* Sets the Ip Address of the Connection Data in Unity Transport
	to the Ip Address which was input in the Input Field */
    // ONLY FOR CLIENT SIDE
    public void SetIpAddress()
    {
        if (ipAddress == null)
        {
            Debug.Log("Ko tim thay ip address");
        }
        else
        {
            Debug.Log("Tim thay ip address:" + ipAddress);
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            if (transport == null)
            {
                Debug.Log("Ko tim thay component transport tu client");
            }
            else
            {
                transport.ConnectionData.Address = ipAddress;
                Debug.Log("IP client lay được: " + transport.ConnectionData.Address);
            }
        }
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