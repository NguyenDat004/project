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
    private Rigidbody2D myRigidbody;
    private float TimeShoot;
    private float TimeCount;
    private Animator shootAnimator;
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        shootAnimator = GetComponent<Animator>();
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
        if (Input.GetKeyDown(KeyCode.Space) && TimeCount >= TimeShoot)
        {
            Shoot();
            TimeCount = 0;
        }
        TimeCount += Time.deltaTime;

    }
    private void Shoot()
    {
        if (gameObject.name == "Cowboy")
        {
            StartCoroutine(PlayAnimationAndWait("ShootingPistol"));
        }
        else if (gameObject.name == "Robot")
        {
            StartCoroutine(PlayAnimationAndWait("ShootingSniper"));
        }
        else if (gameObject.name == "Madman")
        {
            StartCoroutine(PlayAnimationAndWait("Throwing"));
        }

        else if (gameObject.name == "Assassin")
        {
            StartCoroutine(PlayAnimationAndWait("Slash"));
        }

        GameObject newBullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), newBullet.GetComponent<Collider2D>(), true);

        Rigidbody2D bulletRigidbody = newBullet.GetComponent<Rigidbody2D>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = transform.right * bulletSpeed;
        }
    }

    private IEnumerator PlayAnimationAndWait(string animName)
    {
        // Start playing the animation
        shootAnimator.Play(animName);

        // Wait for the Animator to actually transition to the animation state
        yield return new WaitForEndOfFrame();

        // Check animation state and wait until it's done
        while (shootAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName) &&
               shootAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null; // Wait until the next frame and check again
        }

        // Reset or set the animation state
        SetStateAnimation();

        // Animation has completed
        Debug.Log("Animation completed!");
    }

    void SetStateAnimation()
    {
        if (myRigidbody.velocity.y > 1)
        {
            shootAnimator.SetBool("JumpState", true);
            shootAnimator.SetBool("FallingState", false);
            shootAnimator.SetBool("Walking", false);
            shootAnimator.SetBool("Standing", false);
        }
        else if (myRigidbody.velocity.y < -1)
        {
            shootAnimator.SetBool("JumpState", false);
            shootAnimator.SetBool("FallingState", true);
            shootAnimator.SetBool("Walking", false);
            shootAnimator.SetBool("Standing", false);
        }
        else if (myRigidbody.velocity.x != 0)
        {
            shootAnimator.SetBool("JumpState", false);
            shootAnimator.SetBool("FallingState", false);
            shootAnimator.SetBool("Walking", true);
            shootAnimator.SetBool("Standing", false);
        }
        else
        {
            shootAnimator.SetBool("JumpState", false);
            shootAnimator.SetBool("FallingState", false);
            shootAnimator.SetBool("Walking", false);
            shootAnimator.SetBool("Standing", true);
        }
    }
}