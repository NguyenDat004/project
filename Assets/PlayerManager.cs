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
        if (playerPrefabs.Length > 0)
        {
            Debug.Log("Host:Client" + NetworkManager.Singleton.IsHost + ", " + NetworkManager.Singleton.IsClient);
            int i=Random.Range(0, playerPrefabs.Length);
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
            {
                GameObject playerInstance = Instantiate(playerPrefabs[i], transform.position, Quaternion.identity);
                NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

                // Make sure the network object is spawned and assigned to the current client
                if (networkObject != null)
                {
                    networkObject.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
                }
                
            }
        }
        else
        {
            Debug.LogError("Player prefabs array is empty.");
        }

    }
}
