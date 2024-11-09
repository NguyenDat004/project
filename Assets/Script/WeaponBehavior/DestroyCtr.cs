using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCtr : MonoBehaviour
{
    float count = 0;
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 3;
    }
    private void Update()
    {
        count += Time.deltaTime;
        if (count > 0.2)
        {
            Destroy(gameObject);
        }
    }

}
