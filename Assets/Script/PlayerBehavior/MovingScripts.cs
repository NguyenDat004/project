using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class MovingScripts : NetworkBehaviour
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
    private Animator animator;
    float countPrint;
    void Start()
    {
        
        animator = GetComponent<Animator>();
        animator.SetBool("JumpState",false);
        animator.SetBool("FallingState", false);
        animator.SetBool("WalkingState", false);
        animator.SetBool("StandingState", true);

        countPrint = 0;

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

        maxHorizontalSpeed = 40; // Giới hạn tốc độ di chuyển theo trục X|XXXXXXXXXXXXX( QUAN TRỌNG )XXXXXXXXXXXXXXXX

    }

    void Update()
    {
        if (IsOwner)
        {
            CharacterMoving();
            CharacterJumping();
            CharacterFalling();
            SetStateAnimation();
        }
    } 
    void SetStateAnimation()
    {

        if (myRigidbody.velocity.y > 1)
        {

            animator.SetBool("JumpState", true);
            animator.SetBool("FallingState", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Standing", false);
        }
        else if (myRigidbody.velocity.y < -1)
        {

            animator.SetBool("JumpState", false);
            animator.SetBool("FallingState", true);
            animator.SetBool("Walking", false);
            animator.SetBool("Standing", false);
        }
        else if (myRigidbody.velocity.x>20 || myRigidbody.velocity.x<-20 )
        {

            animator.SetBool("JumpState", false);
            animator.SetBool("FallingState", false);
            animator.SetBool("Walking", true);
            animator.SetBool("Standing", false);
        }
        else
        {
            animator.SetBool("JumpState", false);
            animator.SetBool("FallingState", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Standing", true);
        }

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
        timeCountJump-=Time.deltaTime;
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
            WaitFall();
        }

        // Tăng tốc độ rơi khi vận tốc y lớn hơn -50
        if (myRigidbody.velocity.y < -50)
        {
            myRigidbody.velocity -= vecGravity * tocdoroi * Time.deltaTime;
        }
    }

    void WaitFall()
    {
        float i = 0;
        while (i < 0.3f)
        {
            i += Time.deltaTime;
        }
        foreach (var collider in secondGroundColliders)
        {
            Physics2D.IgnoreCollision(collider, this.GetComponent<Collider2D>(), false);
        }
    }

    private void CharacterMoving()
    {
        float horizontalVelocity = myRigidbody.velocity.x;
        if (Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D)){
            

        
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
            // Giới hạn tốc độ theo trục X khi nhấn nhiều phím
            horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxHorizontalSpeed, maxHorizontalSpeed);

            // Gán vận tốc mới cho Rigidbody
            myRigidbody.velocity = new Vector2(horizontalVelocity, myRigidbody.velocity.y);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu va chạm với mặt đất "BaseGround" hoặc "SecondGround"
        if (collision.gameObject.name == "BaseGround" || collision.gameObject.name == "SecondGround")
        {
            if (myRigidbody.velocity.y < 1 && myRigidbody.velocity.y > -1)
            {
                // Reset số lần nhảy khi chạm đất
                jumpCount = 2;
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