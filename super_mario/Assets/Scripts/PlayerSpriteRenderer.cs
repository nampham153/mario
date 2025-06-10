using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] 
public class PlayerSpriteRenderer : MonoBehaviour
{
    private PlayerMovement movement; 
    public SpriteRenderer spriteRenderer { get; private set; } 

    // Các sprite tương ứng với trạng thái của nhân vật
    public Sprite idle;   // Sprite khi nhân vật đứng yên
    public Sprite jump;   // Sprite khi nhân vật nhảy
    public Sprite slide;  // Sprite khi nhân vật trượt
    public AnimatedSprite run; // Hoạt ảnh chạy

    private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

   
    // Hàm LateUpdate được gọi vào cuối , đảm bảo hình ảnh được cập nhật sau khi nhân vật di chuyển.
    private void LateUpdate()
    {
        run.enabled = movement.running; // Nếu nhân vật đang chạy, kích hoạt hoạt ảnh chạy

        // Cập nhật sprite phù hợp với trạng thái 
        if (movement.jumping)
        {
            spriteRenderer.sprite = jump; // Nếu nhân vật nhảy -> hiển thị sprite nhảy
        }
        else if (movement.sliding)
        {
            spriteRenderer.sprite = slide; // Nếu nhân vật trượt, hiển thị sprite trượt
        }
        else if (!movement.running)
        {
            spriteRenderer.sprite = idle; // Nếu nhân vật không chạy, hiển thị sprite đứng yên
        }
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false; 
        run.enabled = false; 
    }
}
