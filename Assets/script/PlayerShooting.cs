using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab viên đạn
    public Transform firePoint;      // Vị trí nơi viên đạn được bắn ra
    public float bulletSpeed = 80f;  // Tốc độ viên đạn
    public float pushForce = 700f;   // Lực đẩy khi đạn va chạm vào kẻ địch

    void Update()
    {
        // Kiểm tra nếu nhấn phím bắn 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Tạo viên đạn từ prefab tại vị trí firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Xác định hướng bắn dựa trên hướng nhân vật
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        // Gọi hàm SetBulletDirection để đạn bắn ra theo hướng đã xác định
        SetBulletDirection(bullet, new Vector2(direction, 0), bulletSpeed);
    }

    void SetBulletDirection(GameObject bullet, Vector2 shootDirection, float speed)
    {
        Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();

        // Gán hướng và vận tốc cho viên đạn
        bulletBody.velocity = shootDirection.normalized * speed;

        // Thiết lập script va chạm cho viên đạn
        BulletCollision bulletCollision = bullet.AddComponent<BulletCollision>();
        bulletCollision.pushForce = pushForce; // Gán lực đẩy khi va chạm
    }

    // Script con điều khiển va chạm của viên đạn
    private class BulletCollision : MonoBehaviour
    {
        public float pushForce;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Object")
            {
                Rigidbody2D enemyBody = collision.GetComponent<Rigidbody2D>();

                if (enemyBody != null)
                {
                    // Tính toán hướng đẩy kẻ địch dựa trên vị trí va chạm
                    Vector2 pushDirection = (collision.transform.position - transform.position).normalized;

                    // Áp dụng lực đẩy cho kẻ địch
                    enemyBody.AddForce(pushDirection * pushForce);
                }

                // Phá hủy viên đạn sau khi va chạm
                Destroy(gameObject);
            }
        }

        // Xử lý hủy đạn khi ra khỏi màn hình
        void Update()
        {
            if (transform.position.y < -55)
            {
                Destroy(gameObject);
            }
        }
    }
}