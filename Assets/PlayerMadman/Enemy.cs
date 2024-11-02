using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Phương thức sát thương
    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
