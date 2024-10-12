using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScripts : MonoBehaviour
{
    public Rigidbody2D myRigidbody;  // Khai báo Rigidbody2D cho đối tượng
    private BoxCollider2D currentCollider; // Collider của đối tượng đang va chạm
    public float upwardForce = 40f;   // nhay khi nhấn phím W
    public float leftForce = 20f;     // sang trai khi nhấn phím A
    public float rightForce = 20f;    // sang phai khi nhấn phím D
    public float downForce = 20f;    // xuong khi nhấn phím s

    private int jumpCount = 2; // Số lần nhảy còn lại
    private bool isGrounded;    // Kiểm tra xem có chạm đất hay không
    private Vector2 originalSize; // Lưu kích thước ban đầu của BoxCollider2D

    void Start()
    {

    }

    void Update()
    {
        // Kiểm tra nếu phím W đang được nhấn
        if (Input.GetKeyDown(KeyCode.W) && jumpCount > 0)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, upwardForce); // nhảy lên
            jumpCount--; // Giảm số lần nhảy
        }
        // Kiểm tra nếu phím S đang được nhấn
         if (Input.GetKeyDown(KeyCode.S) && currentCollider != null)
        {
            originalSize = currentCollider.size;
            // Thay đổi kích thước của BoxCollider2D
            Vector2 newSize = new Vector2(0.0000001f, 0.0000001f); // Kích thước mới cho collider
            currentCollider.size = newSize; // Cập nhật kích thước
            // Bắt đầu coroutine để trở lại kích thước ban đầu
            StartCoroutine(ResetColliderSize(2f));
            // myRigidbody.velocity = new Vector2(-downForce, myRigidbody.velocity.x);
        }
        // Di chuyển và xoay khi nhấn phím A hoặc D
        if (Input.GetKey(KeyCode.A))
        {
            myRigidbody.velocity = new Vector2(-leftForce, myRigidbody.velocity.y); // Di chuyển trái
            transform.localScale = new Vector3(-1f, 1f, 0); // Xoay về bên trái
        }
        else if (Input.GetKey(KeyCode.D))
        {
            myRigidbody.velocity = new Vector2(rightForce, myRigidbody.velocity.y); // Di chuyển phải
            transform.localScale = new Vector3(1f, 1f, 0); // Xoay về bên phải
        }
        else
        {
            // Đặt tốc độ di chuyển x về 0 nếu không nhấn phím A hoặc D
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra va chạm với mặt đất
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Object")
        {
            jumpCount = 2; // Reset số lần nhảy về 2 khi chạm đất
        }
        if (collision.gameObject.tag == "Object")
        {
            currentCollider = null; // Đặt currentCollider về null
            currentCollider = collision.gameObject.GetComponent<BoxCollider2D>(); // Lấy BoxCollider2D
        }
    }
    private IEnumerator ResetColliderSize(float waitTime)
    {
        // Chờ một khoảng thời gian
        yield return new WaitForSeconds(waitTime);
        
        // Trở lại kích thước ban đầu
        if (currentCollider != null)
        {
            currentCollider.size = originalSize; // Đặt kích thước về ban đầu
        }
    }
}
