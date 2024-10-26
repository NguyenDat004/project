using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bom : MonoBehaviour
{
    private AudioManager audioManager;
    private SpriteRenderer genadeRenderer;
    private Rigidbody2D bombRigidbody; // Để điều khiển vật lý của bom

    // Thời gian phát nổ sau khi ném
    private float explosionTime = 3f;
    private bool hasExploded = false; // Đánh dấu bom đã nổ

    // Lực nảy và độ trễ nảy khi chạm đất
    public float bounceForce = 2f;
    public float knockbackForce = 2f;

    // Lực ban đầu và góc ném để tạo quỹ đạo cong
    public float initialThrowForce = 10f; // Tăng lực ném
    public float throwAngle = 50f; // Có thể điều chỉnh giá trị này để thay đổi độ cong

    // Thay đổi trọng lực để điều chỉnh tốc độ rơi
    public float gravityScale = 5f; // Tăng giá trị này để bom rơi nhanh hơn

    private bool isReadyToBounce = true; // Đảm bảo có thể nảy nhiều lần trước khi nổ

    private void Awake()
    {
        genadeRenderer = gameObject.GetComponent<SpriteRenderer>();
        genadeRenderer.sortingOrder = 2;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // Gán Rigidbody2D của bom
        bombRigidbody = GetComponent<Rigidbody2D>();

        // Thiết lập vị trí Z âm
        Vector3 currentPosition = gameObject.transform.position;
        currentPosition.z = -Mathf.Abs(currentPosition.z);
        gameObject.transform.position = currentPosition;
    }

    private void Start()
    {
        // Đặt trọng lực để bom rơi nhanh hơn
        bombRigidbody.gravityScale = 9;

        // Đặt hẹn giờ để bom phát nổ sau 3 giây
        Invoke(nameof(Explode), explosionTime);

        // Tạo quỹ đạo cong ban đầu với góc và vận tốc ném
        //float throwAngleRad = throwAngle * Mathf.Deg2Rad;

/*        // Lấy hướng ném từ hướng của người chơi
        Vector2 throwDirection;
        if (transform.localScale.x < 0) // Nếu nhân vật đang nhìn sang trái
        {
            throwDirection = new Vector2(-Mathf.Cos(throwAngleRad), Mathf.Sin(throwAngleRad));
        }
        else // Nếu nhân vật đang nhìn sang phải
        {
            throwDirection = new Vector2(Mathf.Cos(throwAngleRad), Mathf.Sin(throwAngleRad));
        }

        // Đặt vận tốc ban đầu
        bombRigidbody.velocity = throwDirection.normalized * initialThrowForce;*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Phát âm thanh nổ và đẩy lùi khi bom chạm người chơi
        if (collision.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.bomClip);

            // Đẩy lùi nhân vật
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            Destroy(gameObject);
        }
    }

    // Kiểm soát nảy khi chạm đất
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !hasExploded && isReadyToBounce)
        {
            // Thêm lực nảy khi chạm đất hoặc vật thể, chỉ nảy nhẹ
            bombRigidbody.velocity = new Vector2(bombRigidbody.velocity.x * 0.5f, bounceForce);

            // Đảm bảo bom sẽ không nảy liên tục
            StartCoroutine(BounceCooldown());
        }
    }

    // Giới hạn tần suất nảy
    private IEnumerator BounceCooldown()
    {
        isReadyToBounce = false;
        yield return new WaitForSeconds(0.2f); // Thời gian chờ trước khi có thể nảy lại
        isReadyToBounce = true;
    }

    // Phát nổ sau 3 giây
    private void Explode()
    {
        if (!hasExploded)
        {
            audioManager.PlaySFX(audioManager.bomClip); // Phát âm thanh nổ
            Destroy(gameObject); // Hủy bom sau khi phát nổ
            hasExploded = true;
        }
    }
}
