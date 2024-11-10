using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] playerPrefabs;

    private void Start()
    {
        if (IsHost)
        {
            Debug.Log("Host spawned");
            // The host spawns its own player when the game starts.
            SpawnPlayer(NetworkManager.Singleton.LocalClientId);
        }
        else if (IsClient)
        {
            Debug.Log("Client spawned");
            // Clients request the host to spawn their player when they connect.
            RequestSpawnPlayerServerRpc();
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (playerPrefabs.Length == 0)
        {
            Debug.LogError("Player prefabs array is empty.");
            return;
        }

        int i = Random.Range(0, playerPrefabs.Length);
        GameObject playerInstance = Instantiate(playerPrefabs[i], transform.position, Quaternion.identity);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            // Spawn the player object and assign ownership to the specific client ID
            networkObject.SpawnAsPlayerObject(clientId);
            Debug.Log("Player spawned for client ID: " + clientId);
        }
        else
        {
            Debug.LogError("The instantiated player prefab does not have a NetworkObject component.");
            Destroy(playerInstance);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestSpawnPlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        if (IsHost)
        {
            // The server (host) spawns a player for the client that sent the request.
            SpawnPlayer(rpcParams.Receive.SenderClientId);
        }
    }
}
