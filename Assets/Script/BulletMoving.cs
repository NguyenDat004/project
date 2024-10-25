using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoving : MonoBehaviour
{
    public Rigidbody2D BulletBody;
    public SpriteRenderer BulletRenderer;
    public float speedBullet = 80f;
    public float pushForce = 700f; // Lực đẩy khi đạn va chạm vào kẻ địch

    // Start is called before the first frame update
    void Start()
    {
        // Set default size
        float defScale = 0.5f;
        this.transform.localScale = new Vector3(defScale, defScale, defScale);

        BulletRenderer = GetComponent<SpriteRenderer>();
        BulletRenderer.sortingOrder = 4;

        // Set random color if sprite is LightBulb or LightBall
        // if (BulletRenderer.sprite.name == "LightBulb" || BulletRenderer.sprite.name == "LightBall")
        // {
        //     BulletRenderer.color = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
        // }

        BulletBody = GetComponent<Rigidbody2D>();

        // Set bullet to layer 3
        gameObject.layer = 3;

        ShootBullet();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -55)
        {
            Destroy(gameObject);
        }
    }

    void ShootBullet()
    {
        // Get mouse position in world space
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Calculate direction from bullet to mouse
        Vector2 shootDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;
        // Apply velocity to move the bullet
        BulletBody.velocity = shootDirection * speedBullet;
    }

     // Hàm xử lý khi viên đạn va chạm với các đối tượng khác
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Ground" )
        {
            // Lấy Rigidbody2D của đối tượng kẻ địch
            Rigidbody2D enemyBody = collision.GetComponent<Rigidbody2D>();

            if (enemyBody != null)
            {
                // Tính toán hướng đẩy kẻ địch dựa trên vị trí va chạm
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                
                // Áp dụng lực đẩy cho kẻ địch theo hướng đẩy và với lực định trước
                enemyBody.AddForce(pushDirection * pushForce);
            }

            // Phá hủy viên đạn sau khi va chạm
            Destroy(gameObject);
        }
    }
}
