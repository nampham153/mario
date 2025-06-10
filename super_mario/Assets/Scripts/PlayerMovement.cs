using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb; 
    private Collider2D capsuleCollider; 

    private Vector2 velocity;
    private float inputAxis;

   
    public float moveSpeed = 8f; // Tốc độ di chuyển
    public float maxJumpHeight = 5f; // Chiều cao tối đa của cú nhảy
    public float maxJumpTime = 1f; // Thời gian tối đa cho cú nhảy
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f); // Lực nhảy
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f); // Trọng lực

    // Trạng thái của nhân vật
    public bool grounded { get; private set; } // Kiểm tra nhân vật có  đứng trên mặt đất không
    public bool jumping { get; private set; } // Kiểm tra nhân vật có đang nhảy không
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f; // Kiểm tra nhân vật có đang chạy không
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f); // Kiểm tra nhân vật có đang trượt không
    public bool falling => velocity.y < 0f && !grounded; // Kiểm tra nhân vật có đang rơi không

    private void Awake()
    {
        mainCamera = Camera.main; 
        rb = GetComponent<Rigidbody2D>(); 
        capsuleCollider = GetComponent<Collider2D>(); 
    }

    private void OnEnable()
    {
        rb.isKinematic = false; 
        capsuleCollider.enabled = true; 
        velocity = Vector2.zero;
        jumping = false; 
    }

    private void OnDisable()
    {
        rb.isKinematic = true; 
        capsuleCollider.enabled = false; 
        velocity = Vector2.zero;
        inputAxis = 0f; 
        jumping = false; 
    }


    private void Update()
    {
        HorizontalMovement(); // Xử lý di chuyển ngang

        grounded = rb.Raycast(Vector2.down); // Kiểm tra nhân vật có đang chạm đất không

        if (grounded)
        {
            GroundedMovement(); // Nếu chạm đất, xử lý di chuyển trên mặt đất
        }

        ApplyGravity(); 
    }

    // gọi với tần suất cố định để xử lý di chuyển vật lý.

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = mainCamera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rb.MovePosition(position); 
    }

    // Xử lý di chuyển ngang 
    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal"); 
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime); 

        // Nếu nhân vật va chạm với tường, dừng di chuyển
        if (rb.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        // Xoay nhân vật theo hướng di chuyển
        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    /// Xử lý di chuyển khi nhân vật chạm đất.
    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // Nếu nhấn nút nhảy, áp dụng lực nhảy
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    /// Áp dụng trọng lực cho nhân vật.
    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump"); // Kiểm tra nhân vật có đang rơi không
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime; // Cập nhật vận tốc theo trọng lực
        velocity.y = Mathf.Max(velocity.y, gravity / 2f); 
    }


    // Xử lý va chạm với các vật thể khác.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu va chạm với kẻ địch, kiểm tra hướng va chạm
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f; // Nếu nhảy lên đầu kẻ địch, nhân vật bật lên
                jumping = true;
            }
        }
        // Nếu va chạm với vật thể không phải PowerUp, kiểm tra va chạm từ trên xuống
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f; 
            }
        }
    }
}
