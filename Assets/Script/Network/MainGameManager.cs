using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab của player
    private void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Host is start");
            // Spawn the player prefab on the network
            SpawnPlayer();
        }
        else
        {
            Debug.Log("Host is not start");
        }

    }
    private void SpawnPlayer()
    {
        // Use the NetworkManager to spawn the player prefab
        GameObject player = Instantiate(playerPrefab);
        NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();
        playerNetworkObject.Spawn();  // Spawn the player on the network
        Debug.Log("Player spawned in Scene 2 as"+ (NetworkManager.Singleton.IsClient?"Client":"Server"));
    }
}
