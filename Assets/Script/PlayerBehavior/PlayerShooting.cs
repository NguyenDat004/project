using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerShooting : NetworkBehaviour
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
        firePoint = transform.GetChild(0).gameObject;
        TimeCount = 0;
        TimeShoot = 0.5f;
    }

    private void Update()
    {
        if (!IsOwner) return; // Chỉ cho phép client sở hữu nhân vật mới thực hiện hành động bắn

        // Kiểm tra nếu nhấn phím bắn
        if (Input.GetKeyDown(KeyCode.Space) && TimeCount >= TimeShoot)
        {
            ShootServerRpc();
            TimeCount = 0;
        }
        TimeCount += Time.deltaTime;
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        // Chỉ server mới tạo và đồng bộ hóa viên đạn
        ShootClientRpc();
    }

    [ClientRpc]
    private void ShootClientRpc()
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
        shootAnimator.Play(animName);

        yield return new WaitForEndOfFrame();

        while (shootAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName) &&
               shootAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        SetStateAnimation();

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
