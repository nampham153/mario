using System.Collections;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public Sprite deadSprite; //  hiển thị khi nhân vật chết

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite(); // Cập nhật hình ảnh khi chết
        DisablePhysics(); // Vô hiệu hóa va chạm và chuyển động
        StartCoroutine(Animate()); // Bắt đầu hiệu ứng nhân vật bay lên rồi rơi xuống
    }

    //  được gọi khi GameObject bị tắt
    private void OnDisable()
    {
        StopAllCoroutines(); 
    }

    // Hàm cập nhật hình ảnh khi nhân vật chết
    private void UpdateSprite()
    {
        spriteRenderer.enabled = true; // Đảm bảo nhân vật vẫn hiển thị
        spriteRenderer.sortingOrder = 10; // Đặt thứ tự hiển thị cao hơn để luôn hiển thị trên các đối tượng khác

        if (deadSprite != null)
        {
            spriteRenderer.sprite = deadSprite; // Thay đổi sprite sang hình nhân vật chết
        }
    }

    // Hàm vô hiệu hóa va chạm và chuyển động khi nhân vật chết
    private void DisablePhysics()
    {
        // Tắt tất cả collider để nhân vật không còn va chạm với môi trường
        Collider2D[] colliders = GetComponents<Collider2D>();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        // Nếu có Rigidbody2D đặt chế độ isKinematic để vô hiệu hóa trọng lực và va chạm
        if (TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.isKinematic = true;
        }

        // Nếu có điều khiển di chuyển của người chơi, tắt nó để nhân vật không thể di chuyển
        if (TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.enabled = false;
        }

        // Nếu có  điều khiển di chuyển của kẻ địch, tắt nó để dừng di chuyển
        if (TryGetComponent(out EntityMovement entityMovement))
        {
            entityMovement.enabled = false;
        }
    }

    // tạo hiệu ứng nhân vật chết (nhảy lên rồi rơi xuống)
    private IEnumerator Animate()
    {
        float elapsed = 0f; 
        float duration = 3f;

        float jumpVelocity = 10f; // Vận tốc ban đầu khi nhảy lên
        float gravity = -36f; // Gia tốc trọng lực hướng xuống

        Vector3 velocity = Vector3.up * jumpVelocity;

        // Vòng lặp để tạo hiệu ứng nhân vật nhảy lên rồi rơi xuống
        while (elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime; 
            yield return null; 
        }
    }
}
