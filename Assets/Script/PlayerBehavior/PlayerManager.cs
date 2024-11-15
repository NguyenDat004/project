using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] playerPrefabs; // Array of player prefabs

    private NetworkVariable<FixedString128Bytes> playerCharNetVariable = new NetworkVariable<FixedString128Bytes>();

    // Called when the object is spawned on the network
    public override void OnNetworkSpawn()
    {

        if (IsClient)
        {
            Debug.Log("This is Client On PlayerManager");
            SetPlayerCharacterServerRpc(SetCharacter.selectedCharacterSprite);
        }
        
    }

        // Server RPC to set the player character on the server
        [ServerRpc(RequireOwnership = false)]
    private void SetPlayerCharacterServerRpc(string playerChar, ServerRpcParams rpcParams = default)
    {
        Debug.Log("Setting character on server: " + playerChar);
        playerCharNetVariable.Value = playerChar; // Store the character selection in the network variable

        
        // After setting, spawn the player for the client that owns this character
        SpawnPlayerClientRpc(playerChar, rpcParams.Receive.SenderClientId);
    }
    [ClientRpc]
    // Method to spawn the player object based on character selection
    private void SpawnPlayerClientRpc(string selectedCharacter, ulong clientId)
    {
        Debug.Log("Access SpawnPlayerClientRpc with ID: " + clientId);
        if (playerPrefabs.Length == 0 || !IsHost)
        {
            Debug.LogError("Player prefabs array is empty or not Host");
            return;
        }

        GameObject playerInstance;
        NetworkObject networkObject;

        // Instantiate the correct player prefab based on selected character
        Debug.Log("Selected Character: " + selectedCharacter);

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
            Debug.Log("Client spawned Robot");
        }

        Debug.Log("SPAWNNNNNNNNNNNNNNNNNNN");

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
}
