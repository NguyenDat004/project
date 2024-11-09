using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkManagerSpawn : NetworkManager
{
    public GameObject playerPrefab;  // Tham chiếu tới prefab nhân vật
    public Transform[] spawnPoints;  // Mảng các điểm spawn


}
