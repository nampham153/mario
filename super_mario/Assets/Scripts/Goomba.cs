using UnityEngine;

public class Goomba : MonoBehaviour
{
    // Sprite khi Goomba bị dẫm bẹp
    public Sprite flatSprite;

    // Xử lý khi Goomba va chạm với một đối tượng khác.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm là người chơi
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player player))
        {
            // Nếu người chơi có Star Power, Goomba bị tiêu diệt ngay
            if (player.starpower)
            {
                Hit();
            }
            // Nếu người chơi nhảy trúng Goomba từ trên xuống, nó sẽ bị dẫm bẹp
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }
            // Nếu va chạm từ các hướng khác, người chơi sẽ bị tấn công
            else
            {
                player.Hit();
            }
        }
    }

    // Xử lý khi Goomba bị chạm vào bởi các đối tượng khác.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu Goomba bị một vỏ Koopa va trúng, nó sẽ bị tiêu diệt ngay
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    // Khi Goomba bị dẫm bẹp, nó sẽ mất khả năng va chạm và bị hủy sau một khoảng thời gian ngắn.
    private void Flatten()
    {
        // Vô hiệu hóa collider để Goomba không còn tương tác vật lý
        GetComponent<Collider2D>().enabled = false;

        // Tắt di chuyển và hoạt ảnh của Goomba
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;

        // Thay đổi sprite thành hình dạng bị dẫm bẹp
        GetComponent<SpriteRenderer>().sprite = flatSprite;

        // Hủy Goomba sau 0.5 giây
        Destroy(gameObject, 0.5f);
    }

    // Khi Goomba bị tiêu diệ, nó sẽ kích hoạt hiệu ứng chết và biến mất.
    private void Hit()
    {
        // Tắt hoạt ảnh của Goomba
        GetComponent<AnimatedSprite>().enabled = false;

        // Kích hoạt hiệu ứng chết
        GetComponent<DeathAnimation>().enabled = true;

        // Hủy Goomba sau 3 giây
        Destroy(gameObject, 3f);
    }
}
