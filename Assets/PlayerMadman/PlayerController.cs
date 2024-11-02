using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioManager audioManager;

    public float jumpForce = 5f;
    public LayerMask groundLayer; // Layer mặt đất

    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // Kiểm tra xem nhân vật có chạm đất không, sử dụng vị trí của nhân vật
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
    }
}
