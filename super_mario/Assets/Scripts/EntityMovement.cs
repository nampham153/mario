using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    public float speed = 1f; // Tốc độ di chuyển của thực thể
    public Vector2 direction = Vector2.left; // Hướng di chuyển ban đầu

    private Rigidbody2D rb; 
    private Vector2 velocity; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enabled = false; 
    }

    // Khi thực thể trở nên hiển thị trên màn hình
    private void OnBecameVisible()
    {
#if UNITY_EDITOR
        enabled = !EditorApplication.isPaused;
#else
        enabled = true; // Nếu là bản build, luôn kích hoạt khi nhìn thấy
#endif
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rb.WakeUp(); // Rigidbody2D để có thể nhận tương tác vật lý
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero; // Đặt vận tốc về 0 để dừng chuyển động
        rb.Sleep();
    }

    private void FixedUpdate()
    {
        // Thiết lập vận tốc ngang theo hướng di chuyển và tốc độ
        velocity.x = direction.x * speed;

        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        // Nếu thực thể chạm vào vật cản phía trước, đổi hướng di chuyển
        if (rb.Raycast(direction))
        {
            direction = -direction; // Đảo ngược hướng di chuyển
        }

        // Nếu thực thể chạm đất
        if (rb.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }

        // Quay nhân vật theo hướng di chuyển 
        if (direction.x > 0f)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f); // Quay 180 độ khi đi sang phải
        }
        else if (direction.x < 0f)
        {
            transform.localEulerAngles = Vector3.zero; 
        }
    }
}
