using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab for the player
    public string targetSceneName = "MainGame"; // Scene where player should spawn

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not initialized or available.");
            return;
        }

        // Listen for scene loading
        SceneManager.sceneLoaded += OnSceneLoaded;

        // If the NetworkManager is a host (server), spawn player for the host
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host is started");
            SpawnPlayer(); // Server spawns player
        }

        // If the NetworkManager is a client, ask the host to spawn the player
        else if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Client is started");
            RequestSpawnPlayerClientRpc(); // Request spawn from server
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        // If we loaded the target scene, spawn the player
        if (scene.name == targetSceneName)
        {
            Debug.Log("Spawning player in the scene: " + targetSceneName);
            SpawnPlayer();
        }
        else
        {
            Debug.Log("Not in the target scene. Player will not spawn.");
        }
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned.");
            return;
        }

        // Server is responsible for spawning player
        if (NetworkManager.Singleton.IsHost)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();

            if (playerNetworkObject == null)
            {
                Debug.LogError("The player prefab does not have a NetworkObject component.");
                Destroy(player);
                return;
            }

            // Spawn the player object on the network
            playerNetworkObject.Spawn();

            Debug.Log("Player spawned on the network as Host");
        }
    }

    // This RPC will be called by the client to request the server to spawn a player
    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayerClientRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("Client is requesting player spawn");
        SpawnPlayer(); // Server spawns the player
    }
}
