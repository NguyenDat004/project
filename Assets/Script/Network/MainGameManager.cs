using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab for the player

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not initialized or available.");
            return;
        }

        // Nếu là Host (Server), gọi SpawnPlayer
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host is started");
            SpawnPlayerServer(); // Server sẽ spawn player
        }
        // Nếu là Client, chỉ cho phép request spawn từ server (chúng ta không spawn player trực tiếp từ client)
        else if (NetworkManager.Singleton.IsClient)
        {
            //SpawnPlayerClient(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void SpawnPlayerServer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned.");
            return;
        }

        // Nếu là server (host), spawn player
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();

            if (playerNetworkObject == null)
            {
                Debug.LogError("The player prefab does not have a NetworkObject component.");
                Destroy(player);
                return;
            }

            // Spawn player trên server
            playerNetworkObject.Spawn();

            Debug.Log("Player spawned on the network as Host");
        }
    }
    private void SpawnPlayerClient(ulong clientId)
    {
        if (playerPrefab != null)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkObject networkObject = player.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.SpawnAsPlayerObject(clientId);
            }
            else
            {
                Debug.LogError("Player prefab missing NetworkObject component.");
            }
        }
    }
}
