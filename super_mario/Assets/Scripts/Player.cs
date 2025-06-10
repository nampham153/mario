using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //  dùng để phát hiện va chạm
    public CapsuleCollider2D capsuleCollider { get; private set; }

    // Điều khiển chuyển động của nhân vật
    public PlayerMovement movement { get; private set; }

    // Xử lý hiệu ứng chết của nhân vật
    public DeathAnimation deathAnimation { get; private set; }

    // Renderer hiển thị nhân vật nhỏ
    public PlayerSpriteRenderer smallRenderer;

    // Renderer hiển thị nhân vật lớn
    public PlayerSpriteRenderer bigRenderer;

    // Renderer đang được sử dụng 
    private PlayerSpriteRenderer activeRenderer;

    // Kiểm tra xem nhân vật có đang ở trạng thái lớn không
    public bool big => bigRenderer.enabled;

    // Kiểm tra xem nhân vật có đang chết không
    public bool dead => deathAnimation.enabled;

    // Kiểm tra xem nhân vật có đang trong trạng thái Starpower (bất tử)
    public bool starpower { get; private set; }

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        movement = GetComponent<PlayerMovement>();
        deathAnimation = GetComponent<DeathAnimation>();

        // Mặc định nhân vật bắt đầu ở trạng thái nhỏ
        activeRenderer = smallRenderer;
    }

    /// Xử lý khi nhân vật bị kẻ địch  tấn công.
    /// Nếu nhân vật không bất tử và chưa chết, kiểm tra trạng thái:
    /// - Nếu lớn → Thu nhỏ lại.
    /// - Nếu nhỏ → Chết.

    public void Hit()
    {
        if (!dead && !starpower)
        {
            if (big)
            {
                Shrink(); // Nếu đang lớn, thu nhỏ lại
            }
            else
            {
                Death(); // Nếu đang nhỏ, nhân vật chết
            }
        }
    }

    /// Xử lý khi nhân vật chết. Ẩn cả sprite lớn và nhỏ,
    /// kích hoạt hiệu ứng chết, và reset level sau 3 giây.
    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }


    /// Khi nhân vật nhận power-up để lớn lên.
    /// Thay đổi sprite, điều chỉnh collider, và thực hiện hiệu ứng phóng to.
    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        // Điều chỉnh kích thước collider khi nhân vật lớn
        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    /// Khi nhân vật bị tấn công và thu nhỏ lại.
    /// Thay đổi sprite, điều chỉnh collider, và thực hiện hiệu ứng thu nhỏ.
    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        // Điều chỉnh kích thước collider khi nhân vật nhỏ lại
        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }


    /// Hiệu ứng chuyển đổi giữa trạng thái lớn và nhỏ 
    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f; // Thời gian hiệu ứng

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Cứ mỗi 4 frame thì đổi trạng thái hiển thị sprite
            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        // Đảm bảo chỉ một sprite hiển thị sau hiệu ứng
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    /// Kích hoạt trạng thái Starpower (bất tử trong 10 giây).

    public void Starpower()
    {
        StartCoroutine(StarpowerAnimation());
    }

    /// Hiệu ứng Starpower: Nhân vật đổi màu liên tục trong 10 giây.
    private IEnumerator StarpowerAnimation()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f; // Thời gian bất tử

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Cứ mỗi 4 frame đổi màu nhân vật
            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        // Kết thúc Starpower, trở về màu trắng bình thường
        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }
}
