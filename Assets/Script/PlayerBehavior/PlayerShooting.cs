using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Prefab viên đạn

    // Các tham số có thể được thiết lập trong hàm Start
    private float bulletSpeed; // Tốc độ viên đạn
    private GameObject firePoint; // Vị trí nơi viên đạn được bắn ra

    private float TimeShoot;
    private float TimeCount;
    private void Start()
    {
        // Khởi tạo giá trị cho các biến
        bulletSpeed = 180f;  // Tốc độ viên đạn
        // Lấy vị trí bắn từ con cái đầu tiên
        firePoint = transform.GetChild(0).gameObject;
        TimeCount = 0;
        TimeShoot = 0.5f;
    }

    private void Update()
    {
        // Kiểm tra nếu nhấn phím bắn
        if (Input.GetKeyDown(KeyCode.Space) && TimeCount>=TimeShoot)
        {
            Shoot();
            TimeCount=0;
        }
        TimeCount+=Time.deltaTime;

    }

    private void Shoot()
    {
        // Tạo viên đạn từ prefab tại vị trí firePoint
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);

        // Bỏ qua va chạm giữa người chơi và viên đạn
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), newBullet.GetComponent<Collider2D>(), true);

        // Lấy script BulletPistol để thiết lập shooter
        BulletPistol bulletScript = newBullet.GetComponent<BulletPistol>();

        // Thiết lập vận tốc cho viên đạn
        Rigidbody2D bulletRigidbody = newBullet.GetComponent<Rigidbody2D>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = transform.right * bulletSpeed; // Sử dụng tốc độ viên đạn
        }
    }
}
