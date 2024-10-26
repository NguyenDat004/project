using System.Collections;
using UnityEngine;

public class MovingScripts : MonoBehaviour
{
    public Rigidbody2D myRigidbody;  // Đối tượng Rigidbody2D điều khiển vật lý của nhân vật
    private BoxCollider2D currentCollider; // Collider của đối tượng hiện đang va chạm
    public Collider2D[] secondGroundColliders; // Danh sách collider của "SecondGround"

    public float upwardForce;   // Lực nhảy khi nhấn phím W
    public float leftForce;     // Lực sang trái khi nhấn phím A
    public float rightForce;    // Lực sang phải khi nhấn phím D
    public float downForce;     // Lực đi xuống khi nhấn phím S
    public float speed;         // Tốc độ di chuyển của nhân vật
    public float maxHorizontalSpeed; // Tốc độ tối đa theo trục X


    [SerializeField]
    private int jumpCount;  // Số lần nhảy còn lại trước khi phải chạm đất
    private Vector2 originalSize;  // Kích thước ban đầu của BoxCollider2D
    [SerializeField]
    private float tocdoroi;  // Tốc độ rơi của nhân vật khi nhảy lên
    private Vector2 vecGravity;  // Vector đại diện cho trọng lực
    private bool facingRight;

    private float timeCountJump; // Đếm ngược thời gian nhảy

    void Start()
    {
        // Khởi tạo giá trị cho các biến
        upwardForce = 80f;
        leftForce = 30f;
        rightForce = 30f;
        downForce = 20f;
        speed = 3f;
        jumpCount = 2;
        tocdoroi = 3f;

        myRigidbody = GetComponent<Rigidbody2D>();
        secondGroundColliders = GameObject.FindWithTag("SecondGround").GetComponents<Collider2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        facingRight = true;
        timeCountJump = 0;

        maxHorizontalSpeed = 90;
    }

    void Update()
    {
        CharacterMoving();
        CharacterJumping();
        CharacterFalling();

    }

    //Hàm nhảy
    private void CharacterJumping()
    {
        if (Input.GetKeyDown(KeyCode.W) && jumpCount > 0 && myRigidbody.velocity.y > -40)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, upwardForce);
            jumpCount--;
            timeCountJump = 0.2f;
        }
        if (myRigidbody.velocity.y == 0)
        {
            if (timeCountJump <= 0)
            {
                jumpCount = 2;
            }
            else
            {
                timeCountJump -= Time.deltaTime;
            }
        }
    }

    //Hàm rớt
    private void CharacterFalling()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -upwardForce);
            foreach (var collider in secondGroundColliders)
            {
                Physics2D.IgnoreCollision(collider, this.GetComponent<Collider2D>(), true);
            }
        }

        if (myRigidbody.velocity.y < -50)
        {
            myRigidbody.velocity -= vecGravity * tocdoroi * Time.deltaTime;
        }
    }

    private void CharacterMoving()
    {
        float horizontalVelocity = myRigidbody.velocity.x;

        if (Input.GetKey(KeyCode.A))
        {
            if (facingRight)
            {
                flip();
            }
            horizontalVelocity = -leftForce * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!facingRight)
            {
                flip();
            }
            horizontalVelocity = rightForce * speed;
        }
        else
        {
            if (horizontalVelocity < 100 && horizontalVelocity > 0)
            {
                horizontalVelocity -= speed;
            }
            else if (horizontalVelocity < -1 && horizontalVelocity > -100)
            {
                horizontalVelocity += speed;
            }
        }

        horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxHorizontalSpeed, maxHorizontalSpeed);
        myRigidbody.velocity = new Vector2(horizontalVelocity, myRigidbody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BaseGround" || collision.gameObject.name == "SecondGround")
        {
            if (myRigidbody.velocity.y == 0)
            {
                jumpCount = 2;
            }
            foreach (var collider in secondGroundColliders)
            {
                Physics2D.IgnoreCollision(collider, this.GetComponent<Collider2D>(), false);
            }
        }
    }

    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


}