using System.Collections;
using UnityEngine;

public class Bom : MonoBehaviour
{
    private SpriteRenderer grenadeRenderer;
    private Rigidbody2D bombRigidbody;

    public GameObject explosionEffectPrefab;
    public float explosionRadius = 5f;
    public float knockbackForce = 5f;
    public float bounceForce = 2f;
    public float initialThrowForce = 10f;
    public float throwAngle = 50f;
    public float gravityScale = 9f;
    public float explosionTime = 3f;

    private bool hasExploded = false;
    private bool isReadyToBounce = true;

    private void Awake()
    {
        grenadeRenderer = GetComponent<SpriteRenderer>();
        grenadeRenderer.sortingOrder = 2;
        bombRigidbody = GetComponent<Rigidbody2D>();

        Vector3 currentPosition = transform.position;
        currentPosition.z = -Mathf.Abs(currentPosition.z);
        transform.position = currentPosition;
    }

    private void Start()
    {
        bombRigidbody.gravityScale = gravityScale;
        Invoke(nameof(Explode), explosionTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (collision.gameObject.CompareTag("Ground") && !hasExploded && isReadyToBounce)
        {
            bombRigidbody.velocity = new Vector2(bombRigidbody.velocity.x * 0.5f, bounceForce);
            StartCoroutine(BounceCooldown());
        }
    }

    private IEnumerator BounceCooldown()
    {
        isReadyToBounce = false;
        yield return new WaitForSeconds(0.2f);
        isReadyToBounce = true;
    }

    private void Explode(){
        if (!hasExploded)
        {
            hasExploded = true;

            if (explosionEffectPrefab != null)
            {
                GameObject explosionEffect = Instantiate(explosionEffectPrefab, this.transform.position, Quaternion.identity);
                Destroy(explosionEffect, 0.2f);
            }

            Destroy(gameObject);
        }
    }

    public void TriggerExplosion()
    {
        Explode();
    }
}
