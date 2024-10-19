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

        //Gắn rigidbody của nhân vật
        myRigidbody = GetComponent<Rigidbody2D>();
        // Lấy các Collider của đối tượng có tag "SecondGround"
        secondGroundColliders = GameObject.FindWithTag("SecondGround").GetComponents<Collider2D>();

        // Đặt vector trọng lực bằng giá trị trọng lực của Physics2D
        vecGravity = new Vector2(0, -Physics2D.gravity.y);

        // Đặt kích thước mặc định và trạng thái hướng của nhân vật
        transform.localScale = Vector2.one;
        facingRight = true;
        timeCountJump = 0;

        maxHorizontalSpeed = 90; // Giới hạn tốc độ di chuyển theo trục X|XXXXXXXXXXXXX( QUAN TRỌNG )XXXXXXXXXXXXXXXX

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
        // Kiểm tra nếu nhấn phím W và nhân vật còn số lần nhảy
        if (Input.GetKeyDown(KeyCode.W) && jumpCount > 0 && myRigidbody.velocity.y > -40)
        {
            // Tạo vận tốc nhảy lên
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, upwardForce);
            jumpCount--; // Giảm số lần nhảy sau khi thực hiện
            timeCountJump = 0.2f;
        }
        // Nếu vận tốc y nhỏ hơn 50, làm giảm dần vận tốc nhảy lên
        if (myRigidbody.velocity.y < 10 && myRigidbody.velocity.y > 5)
        {
            myRigidbody.velocity = Vector2.zero;
        }
        if (myRigidbody.velocity.y == 0)
        {
            if (timeCountJump <= 0)
            {
                // Reset số lần nhảy khi chạm đất
                jumpCount = 2;

            }
            else
            {
                timeCountJump-=Time.deltaTime;
            }

        }
        Debug.Log(timeCountJump);
    }

    //Hàm rớt
    private void CharacterFalling()
    {
        // Kiểm tra nếu nhấn phím S để làm nhân vật rơi nhanh hơn
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Tăng tốc độ rơi xuống và bỏ qua va chạm với "SecondGround"
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -upwardForce);
            foreach (var collider in secondGroundColliders)
            {
                Physics2D.IgnoreCollision(collider, this.GetComponent<Collider2D>(), true);
            }
        }

        // Tăng tốc độ rơi khi vận tốc y lớn hơn -50
        if (myRigidbody.velocity.y < -50)
        {
            myRigidbody.velocity -= vecGravity * tocdoroi * Time.deltaTime;
        }
    }

    private void CharacterMoving()
    {
        float horizontalVelocity = myRigidbody.velocity.x;

        // Di chuyển nhân vật khi nhấn phím A hoặc D
        if (Input.GetKey(KeyCode.A))
        {
            if (facingRight)  // Nếu nhân vật đang đối mặt phải, lật sang trái
            {
                flip();
            }
            // Di chuyển về bên trái
            horizontalVelocity = -leftForce * speed;
            if (horizontalVelocity > -30)
            {
                horizontalVelocity = 0;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!facingRight)  // Nếu nhân vật đang đối mặt trái, lật sang phải
            {
                flip();
            }
            // Di chuyển về bên phải
            horizontalVelocity = rightForce * speed;
        }
        else
        {
            if(horizontalVelocity < 100 && horizontalVelocity > 0)
            {
                horizontalVelocity -= speed;
            }
            else if (horizontalVelocity<-1 && horizontalVelocity > -100)
            {
                horizontalVelocity += speed;
            }

        }

        // Giới hạn tốc độ theo trục X khi nhấn nhiều phím
        horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxHorizontalSpeed, maxHorizontalSpeed);

        // Gán vận tốc mới cho Rigidbody
        myRigidbody.velocity = new Vector2(horizontalVelocity, myRigidbody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu va chạm với mặt đất "BaseGround" hoặc "SecondGround"
        if (collision.gameObject.name == "BaseGround" || collision.gameObject.name == "SecondGround")
        {
            if (myRigidbody.velocity.y == 0)
            {
                // Reset số lần nhảy khi chạm đất
                jumpCount = 2;
            }
            // Bật lại va chạm với "SecondGround"
            foreach (var collider in secondGroundColliders)
            {
                Physics2D.IgnoreCollision(collider, this.GetComponent<Collider2D>(), false);
            }
        }
    }

    void flip()
    {
        // Đảo ngược trạng thái hướng và lật nhân vật mà không xoay hình
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);  // Xoay đối tượng 180 độ quanh trục y
    }

    // Hàm coroutine để chờ một khoảng thời gian trước khi reset kích thước của collider
    private IEnumerator ResetColliderSize(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Chờ thời gian chỉ định

        // Trở lại kích thước ban đầu nếu currentCollider khác null
        if (currentCollider != null)
        {
            currentCollider.size = originalSize;
        }
    }
}
