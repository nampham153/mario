using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    // Hàm Start() được gọi -> GameObject được tạo
    private void Start()
    {
        //  để tăng số lượng coin
        GameManager.Instance.AddCoin();

        // Bắt đầu thực hiện hiệu ứng di chuyển của đồng xu
        StartCoroutine(Animate());
    }

    // Coroutine thực hiện hiệu ứng di chuyển lên và xuống trước khi hủy đối tượng
    private IEnumerator Animate()
    {
        // Lưu vị trí ban đầu của đồng xu
        Vector3 restingPosition = transform.localPosition;

        Vector3 animatedPosition = restingPosition + Vector3.up * 2f;
        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        // Xóa GameObject
        Destroy(gameObject);
    }

    // Coroutine thực hiện di chuyển giữa 2 điểm trong khoảng thời gian nhất định
    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f; 
        float duration = 0.25f;


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
