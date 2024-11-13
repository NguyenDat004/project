using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] playerPrefabs; // Array of player prefabs


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

        GameObject playerInstance;
        NetworkObject networkObject;

        // Use the selected character sprite name from the NetworkVariable
        string selectedCharacter = SetCharacter.selectedCharacterSprite;

        // Instantiate the correct player prefab based on the selected character name
        if (selectedCharacter == "Assassin")
        {
            playerInstance = Instantiate(playerPrefabs[0], transform.position, Quaternion.identity);
            Debug.Log("Client spawned Assassin");
        }
        else if (selectedCharacter == "Cowboy")
        {
            playerInstance = Instantiate(playerPrefabs[1], transform.position, Quaternion.identity);
            Debug.Log("Client spawned Cowboy");
        }
        else if (selectedCharacter == "Madman")
        {
            playerInstance = Instantiate(playerPrefabs[2], transform.position, Quaternion.identity);
            Debug.Log("Client spawned Madman");
        }
        else
        {
            playerInstance = Instantiate(playerPrefabs[3], transform.position, Quaternion.identity);
            Debug.Log("Client spawned Default");
        }

        networkObject = playerInstance.GetComponent<NetworkObject>();

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
        Debug.Log("Client requested spawn");
        if (IsHost)
        {
            // The server (host) spawns a player for the client that sent the request.
            SpawnPlayer(rpcParams.Receive.SenderClientId);
        }
    }
}
