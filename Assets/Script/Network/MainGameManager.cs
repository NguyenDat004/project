using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab của nhân vật
    public GameObject rocketSpawnerPrefab; // Prefab của nhân vật
    public string targetSceneName = "MainGame"; // Cảnh mà nhân vật sẽ xuất hiện

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager chưa được khởi tạo hoặc không khả dụng.");
            return;
        }

        // Lắng nghe sự kiện tải cảnh
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Khi host hoặc client vào đúng cảnh, spawn nhân vật
        if (NetworkManager.Singleton.IsHost && SceneManager.GetActiveScene().name == targetSceneName)
        {
            Debug.Log("Host đã được khởi động");
            SpawnPlayer(NetworkManager.Singleton.LocalClientId);
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            RequestSpawnPlayerServerRpc();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Cảnh đã được tải: " + scene.name);

        // Nếu tải đúng cảnh, spawn nhân vật
        if (scene.name == targetSceneName)
        {
            Debug.Log("Spawn nhân vật trong cảnh: " + targetSceneName);
            Instantiate(rocketSpawnerPrefab, new Vector3(0, 221, 1), this.transform.rotation);

            if (NetworkManager.Singleton.IsHost)
            {
                SpawnPlayer(NetworkManager.Singleton.LocalClientId);
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                RequestSpawnPlayerServerRpc();
            }
        }
        else
        {
            Debug.Log("Không ở cảnh mục tiêu. Nhân vật sẽ không được spawn.");
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab chưa được gán.");
            return;
        }

        GameObject player = Instantiate(playerPrefab);
        NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();

        if (playerNetworkObject == null)
        {
            Debug.LogError("Prefab của nhân vật không có thành phần NetworkObject.");
            Destroy(player);
            return;
        }

        // Spawn nhân vật với quyền sở hữu gán cho client
        playerNetworkObject.SpawnWithOwnership(clientId);

        Debug.Log($"Nhân vật đã spawn trên mạng cho client {clientId}");
    }

    // RPC này sẽ được client gọi để yêu cầu server spawn nhân vật
    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        Debug.Log($"Client {clientId} yêu cầu spawn nhân vật");
        SpawnPlayer(clientId); // Server spawn nhân vật với quyền sở hữu gán cho client
    }
}
