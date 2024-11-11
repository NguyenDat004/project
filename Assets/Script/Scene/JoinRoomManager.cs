using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System;

public class JoinRoomManager : MonoBehaviour
{
    public Image characterImage;
    public Text characterNameText;
    public Text playerNameText;
    public TMP_Text roomNameText;
    public Sprite[] characters;
    public string[] characterNames;

    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private string ipAddress;
    public UnityTransport transport;
    public TMP_InputField IPAddressHost;
    public TMP_Text IPAddressHostOnScreen;

    void Start()
    {
        ipAddress = "0.0.0.0";
        SetIpAddress();

        hostButton.onClick.AddListener(() =>
        {
            Debug.Log("Host button pressed in Scene 1.");
            StartCoroutine(LoadSceneAndStartHost("MainGame"));
        });

        clientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client button pressed in Scene 1.");
            ipAddress = IPAddressHost.text;
            ConnectToServer(ipAddress);
            SetIpAddress();
            StartCoroutine(LoadSceneAndStartClient("MainGame"));
        });

        if (PlayerPrefs.HasKey("RoomName"))
        {
            roomNameText.text = "Room: " + PlayerPrefs.GetString("RoomName");
        }
        else
        {
            roomNameText.text = "No Room Selected";
        }

        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        if (selectedIndex >= 0 && selectedIndex < characters.Length)
        {
            characterImage.sprite = characters[selectedIndex];
            characterNameText.text = characterNames[selectedIndex];
        }
        else
        {
            Debug.LogError("Selected character index is out of bounds.");
        }

        playerNameText.text = PlayerPrefs.GetString("PlayerName", "Player");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator LoadSceneAndStartHost(string sceneName)
    {
        NetworkManager.Singleton.StartHost();
        ipAddress = GetLocalIPAddress();
        IPAddressHostOnScreen.text = ipAddress;
        Debug.Log("IP address Host on screen assigned: " + IPAddressHostOnScreen.text);

        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host ID: " + NetworkManager.Singleton.LocalClientId.ToString());
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Host started in MainGame scene.");
    }

    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                IPAddressHostOnScreen.text = ip.ToString();
                ipAddress = ip.ToString();
                Debug.Log("IP of host is: " + ipAddress);
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    private IEnumerator LoadSceneAndStartClient(string sceneName)
    {
        NetworkManager.Singleton.StartClient();
        ipAddress = IPAddressHost.text;
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        IPAddressHostOnScreen.text = ipAddress;
        transport.ConnectionData.Address = ipAddress;

        Debug.Log("IP address assigned for transport: " + transport.ConnectionData.Address);

        if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Client ID: " + NetworkManager.Singleton.LocalClientId.ToString());
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Client started in MainGame scene.");
    }

    public void SetIpAddress()
    {
        if (ipAddress == null)
        {
            Debug.Log("IP address not found.");
        }
        else
        {
            Debug.Log("Found IP address: " + ipAddress);
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            if (transport == null)
            {
                Debug.Log("Transport component not found on client.");
            }
            else
            {
                transport.ConnectionData.Address = ipAddress;
                Debug.Log("Client IP assigned: " + transport.ConnectionData.Address);
            }
        }
    }

    public async void ConnectToServer(string ipAddress)
    {
        if (!IPAddress.TryParse(ipAddress, out IPAddress ip))
        {
            Debug.LogError("Invalid IP address: " + ipAddress);
            return;
        }

        if (await PingAddress(ip))
        {
            Debug.Log("IP address is valid and reachable: " + ipAddress);
        }
        else
        {
            Debug.LogError("Unable to connect to IP address: " + ipAddress);
        }
    }

    private async Task<bool> PingAddress(IPAddress ipAddress)
    {
        try
        {
            using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
            {
                PingReply reply = await ping.SendPingAsync(ipAddress.ToString(), 1000);
                return reply.Status == IPStatus.Success;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error during ping: " + e.Message);
            return false;
        }
    }
}
