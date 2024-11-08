using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioManager audioManager;

    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // Kiểm tra xem nhân vật có chạm đất không
        isGrounded = Physics2D.OverlapCircle(transform.position + Vector3.down * 0.5f, 0.1f, groundLayer);

        // Nếu nhấn nút nhảy và nhân vật đang ở trên mặt đất, gọi hàm nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        audioManager.PlayJumpSFX(); // Phát âm thanh nhảy
        Debug.Log("Jumping"); // Kiểm tra xem hàm Jump có được gọi
    }
}