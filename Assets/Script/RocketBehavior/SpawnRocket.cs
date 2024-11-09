using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnRocket : MonoBehaviour
{
    // Start is called before the first frame update
    private float countTime;
    private float timeDrop;//Thời gian để rocket xuất hiện
    [SerializeField]
    GameObject[] rocket;

    void Start()
    {
        countTime = 0;
        timeDrop=5;
    }

    // Update is called once per frame
    void Update()
    {
        if (countTime > timeDrop)
        {
            DropRocket();
            countTime = 0;
        }
        countTime += Time.deltaTime;
    }
    void DropRocket()
    {
        int i= Random.Range(0, rocket.Length);
        Vector3 positionSpawn= new Vector3(Random.Range(-176,176),221, 1);
        Instantiate(rocket[i],positionSpawn,transform.rotation);
    }

}
