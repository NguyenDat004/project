using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject rocketSpawnerPrefab;
    public string targetSceneName = "MainGame";

    private void Start()
    {
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
        Debug.Log("Cảnh đã được tải: " + scene.name);

        if (scene.name == targetSceneName)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Rocket spawn");
                Instantiate(rocketSpawnerPrefab, new Vector3(0, 221, 1), this.transform.rotation);
                SpawnPlayer(NetworkManager.Singleton.LocalClientId); // Host spawn player  
            }
            else
            {
                RequestSpawnPlayerServerRpc();
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
        Debug.Log("SpawnPlayer được gọi");
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab chưa được gán.");
            return;
        }

        // Instantiate đối tượng mới từ prefab  
        GameObject player = Instantiate(playerPrefab);
        Debug.Log("Player prefab được instantiate");

        player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId); ;
        Debug.Log("playernetworkobject spawn with ownership: "+clientId);

        //if (playerNetworkObject == null)
        //{
        //    Debug.LogError("Prefab của nhân vật không có thành phần NetworkObject.");
        //    Destroy(player); // Hủy đối tượng đã tạo nếu không có NetworkObject  
        //    return;
        //}
        //Debug.Log("Da vuot qua check networkobject");
        //// Đặt vị trí spawn cho nhân vật  
        //Vector3 spawnPosition = new Vector3(0, 1, 0); // Thay đổi giá trị này theo cần thiết  
        //player.transform.position = spawnPosition;
        //Debug.Log("Da set vi tri spawn");

        //// Spawn đối tượng mạng với quyền sở hữu cho clientId  
        //playerNetworkObject.Spawn();
        Debug.Log($"Nhân vật đã spawn trên mạng cho client {clientId}");
    }
}