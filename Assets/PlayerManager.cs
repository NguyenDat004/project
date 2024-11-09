using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerPrefabs;

    private void Start()
    {
        // Kiểm tra nếu NetworkManager đã được khởi tạo và đảm bảo chỉ server chạy code này
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not initialized or this is not a server.");
            return;
        }
        if ( !NetworkManager.Singleton.IsServer)
        {
            Debug.LogError("Not server");
            //return;
        }

        if (playerPrefabs.Length > 0)
        {
            Debug.Log("Host:Client - " + NetworkManager.Singleton.IsHost + ", " + NetworkManager.Singleton.IsClient);
            int i = Random.Range(0, playerPrefabs.Length);

            // Spawn nhân vật chỉ khi đang là server/host
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient)
            {
                GameObject playerInstance = Instantiate(playerPrefabs[i], transform.position, Quaternion.identity);
                NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

                if (networkObject != null)
                {
                    // Spawn và gán quyền sở hữu cho client tương ứng
                    networkObject.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
                    Debug.Log("Player spawned for client ID: " + NetworkManager.Singleton.LocalClientId);
                }
                else
                {
                    Debug.LogError("The instantiated player prefab does not have a NetworkObject component.");
                }
            }
        }
        else
        {
            Debug.LogError("Player prefabs array is empty.");
        }
    }
}
