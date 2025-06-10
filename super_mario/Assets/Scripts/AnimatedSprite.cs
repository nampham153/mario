using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    // Mảng để tạo animation
    public Sprite[] sprites;

    // Tốc độ khung hình 1 frame mỗi 1/6 giây
    public float framerate = 1f / 6f;
    private SpriteRenderer spriteRenderer;
    private int frame;

    // Hàm được gọi ngay khi GameObject khởi tạo
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Gọi hàm Animate() liên tục sau mỗi khoảng thời gian framerate
        InvokeRepeating(nameof(Animate), framerate, framerate);
    }

    // GameObject bị tắt
    private void OnDisable()
    {
        CancelInvoke(); // Hủy gọi hàm Animate()
    }

    // thực hiện việc thay đổi  để tạo animation
    private void Animate()
    {
        frame++;
        if (frame >= sprites.Length)
        {
            frame = 0;
        }
        if (frame >= 0 && frame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frame];
        }
    }
}
