using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject rocketSpawnerPrefab;
    public string targetSceneName = "MainGame";

  
    private void Start()
    {

        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Thiết bị đang chạy ở chế độ Host");
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Thiết bị đang chạy ở chế độ Client");
        }
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager chưa được khởi tạo hoặc không khả dụng.");
            return;
        }
        else
        {
            Debug.Log("NetworkManager bth");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                SpawnPlayer(NetworkManager.Singleton.LocalClientId); // Host spawn player
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                Debug.Log("Client in step require spawn");
                //RequestSpawnPlayerServerRpc(); // Client gửi yêu cầu spawn
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        Debug.Log($"Client {clientId} yêu cầu spawn nhân vật");
        SpawnPlayer(clientId); // Gọi spawn trên Server  
    }



    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayer(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            Debug.LogError("Chỉ server mới có thể spawn đối tượng mạng.");
            
        }

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

        // Chỉ server mới có thể gọi SpawnWithOwnership
        playerNetworkObject.SpawnWithOwnership(clientId);

        Debug.Log($"Nhân vật đã spawn trên mạng cho client {clientId}");
    }

}