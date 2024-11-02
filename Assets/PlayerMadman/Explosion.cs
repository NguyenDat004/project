using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //Ban kinh cua vu no
    public float explosionRadius;
    //Kiem tra o layer nao tren game 
    public LayerMask enemyLayer;

    public void DealDamage()
    {
        // Tìm tất cả enemy trong bán kính của vụ nổ
        Collider2D[] findeds = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        if (findeds == null || findeds.Length <= 0) return;

        for (int i = 0; i < findeds.Length; i++)
        {
            var finded = findeds[i];
            if (!finded) continue;
            var enemy = finded.GetComponent<Enemy>();
            if (!enemy) continue;
            enemy.TakeDamage();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
