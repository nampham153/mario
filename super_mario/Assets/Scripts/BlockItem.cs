using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        // thực hiện hiệu ứng xuất hiện của vật phẩm
        StartCoroutine(Animate());
    }

    //  tạo hiệu ứng vật phẩm xuất hiện từ khối
    private IEnumerator Animate()
    {
      
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>(); 
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>(); 
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>(); 
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); 

        // Vô hiệu hóa tương tác vật lý của vật phẩm ban đầu
        rigidbody.isKinematic = true; // Ngăn vật phẩm bị ảnh hưởng bởi trọng lực hoặc va chạm
        physicsCollider.enabled = false;
        triggerCollider.enabled = false; 
        spriteRenderer.enabled = false; // Ẩn vật phẩm khi chưa xuất hiện

        // Chờ 0.25 giây trước khi vật phẩm bắt đầu xuất hiện
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.enabled = true;

        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPosition = transform.position; // Vị trí ban đầu của vật phẩm
        Vector3 endPosition = transform.position + Vector3.up; 
        // Thực hiện hiệu ứng vật phẩm di chuyển lên trên
        while (elapsed < duration)
        {
            float t = elapsed / duration; 

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime; 

            yield return null;
        }

        // Kích hoạt lại các thành phần vật lý sau khi vật phẩm đã xuất hiện hoàn toàn
        rigidbody.isKinematic = false; // Cho phép vật phẩm bị ảnh hưởng bởi trọng lực
        physicsCollider.enabled = true; 
        triggerCollider.enabled = true; 
    }
}
