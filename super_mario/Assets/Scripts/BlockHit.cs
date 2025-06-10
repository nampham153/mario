using System.Collections; 
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item; // Vật phẩm sẽ xuất hiện khi khối bị đập
    public Sprite emptyBlock; // khối khi đã bị đập hết
    public int maxHits = -1; // Số lần có thể bị đập -1 là vô hạn

    private bool animating; 
    // Hàm xử lý khi có va chạm với khối
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Chỉ xử lý nếu khối không đang hoạt ảnh
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            // Kiểm tra va chạm có đến từ phía dưới không 
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Hit(); //  xử lý khi bị đập
            }
        }
    }

    // Hàm xử lý khi khối bị đập
    private void Hit()
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // Đảm bảo khối được hiển thị

        // Giảm số lần có thể đập
        maxHits--;

        // Nếu khối không còn lượt đập, đổi sang sprite khối rỗng
        if (maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }

        // Nếu có vật phẩm  tạo ra item tại vị trí của khối
        if (item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }

        //tạo hiệu ứng khối bị đập nảy lên rồi trở về vị trí cũ
        StartCoroutine(Animate());
    }

    // tạo hiệu ứng khối nảy lên khi bị đập
    private IEnumerator Animate()
    {
        animating = true; // Đánh dấu block đang thực hiện animation

        Vector3 restingPosition = transform.localPosition; // Lưu vị trí ban đầu
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f; // Vị trí khi khối nảy lên

        // Di chuyển khối lên
        yield return Move(restingPosition, animatedPosition);

        // Di chuyển khối trở về vị trí ban đầu
        yield return Move(animatedPosition, restingPosition);

        animating = false; // Kết thúc animation
    }

    //  di chuyển khối giữa hai vị trí với hiệu ứng mượt
    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f; 
        float duration = 0.125f; 

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t); 
            elapsed += Time.deltaTime;

            yield return null; 
        }

        transform.localPosition = to; 
    }
}
