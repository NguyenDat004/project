using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionBomb : MonoBehaviour
{
    public AudioSource explosionSound;

    void Start()
    {
        explosionSound = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu bom va chạm với một đối tượng khác
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "SecondGround")
        {
            explosionSound.Play();
            // Thêm các hiệu ứng khác như phá hủy bom, tạo hiệu ứng hình ảnh, v.v.
            Destroy(gameObject, explosionSound.clip.length); // Phá hủy đối tượng sau khi âm thanh kết thúc
        }
    }
}
