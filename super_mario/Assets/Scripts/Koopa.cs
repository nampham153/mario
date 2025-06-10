using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;

    // Tốc độ di chuyển khi Koopa bị đẩy đi ở trạng thái vỏ
    public float shellSpeed = 12f;

    // Trạng thái Koopa đã bị thu vào vỏ hay chưa
    private bool shelled;

    // Trạng thái Koopa đã bị đẩy đi hay chưa
    private bool pushed;

    /// Xử lý va chạm của Koopa với người chơi.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu Koopa chưa vào trạng thái vỏ và va chạm với người chơi
        if (!shelled && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player player))
        {
            // Nếu người chơi có Star Power (bất tử), Koopa bị tiêu diệt ngay lập tức
            if (player.starpower)
            {
                Hit();
            }
            // Nếu người chơi nhảy trúng Koopa từ trên xuống, Koopa sẽ chui vào vỏ
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }
            // Nếu va chạm từ các hướng khác, người chơi sẽ bị tấn công
            else
            {
                player.Hit();
            }
        }
    }


    //Xử lý sự kiện khi Koopa bị chạm vào bởi các đối tượng khác.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu Koopa đã vào vỏ và bị người chơi chạm vào
        if (shelled && other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            if (!pushed)
            {
                // Nếu vỏ chưa được đẩy, người chơi sẽ đẩy nó đi
                Vector2 direction = new(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                // Nếu vỏ đã được đẩy, nó có thể tấn công người chơi
                if (player.starpower)
                {
                    Hit();
                }
                else
                {
                    player.Hit();
                }
            }
        }
        // Nếu Koopa chưa vào vỏ và bị một vỏ khác va vào, nó sẽ bị tiêu diệt
        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    /// Khi người chơi nhảy trúng Koopa từ trên xuống, Koopa sẽ chui vào vỏ.
    private void EnterShell()
    {
        shelled = true;

        // Thay đổi sprite của Koopa thành vỏ
        GetComponent<SpriteRenderer>().sprite = shellSprite;

        // Tắt hiệu ứng hoạt ảnh và di chuyển của Koopa
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
    }

    /// Khi người chơi đá vỏ Koopa, nó sẽ di chuyển theo hướng được đẩy.
    private void PushShell(Vector2 direction)
    {
        pushed = true;

        // Kích hoạt lại vật lý để Koopa có thể di chuyển
        GetComponent<Rigidbody2D>().isKinematic = false;

        // Điều chỉnh hướng và tốc độ di chuyển của Koopa
        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        // Đổi layer để phân biệt vỏ Koopa với Koopa thường
        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    /// Tiêu diệt Koopa khi bị vỏ khác hoặc người chơi có Star Power tấn công.
    private void Hit()
    {
        // Tắt hoạt ảnh của Koopa
        GetComponent<AnimatedSprite>().enabled = false;

        // Kích hoạt hiệu ứng chết
        GetComponent<DeathAnimation>().enabled = true;

        // Hủy đối tượng Koopa sau 3 giây
        Destroy(gameObject, 3f);
    }

    /// Nếu vỏ Koopa đã bị đẩy ra ngoài màn hình, nó sẽ bị hủy.
    private void OnBecameInvisible()
    {
        if (pushed)
        {
            Destroy(gameObject);
        }
    }
}
