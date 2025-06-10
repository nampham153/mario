using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Điểm kết nối của ống dẫn (nơi người chơi sẽ xuất hiện sau khi đi vào)
    public Transform connection;

    // Phím bấm để vào ống (mặc định là phím S)
    public KeyCode enterKeyCode = KeyCode.S;

    // Hướng mà nhân vật đi vào ống
    public Vector3 enterDirection = Vector3.down;

    // Hướng mà nhân vật thoát ra khỏi ống 
    public Vector3 exitDirection = Vector3.zero;

    //Kiểm tra xem người chơi có đứng trong phạm vi của ống không.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.CompareTag("Player"))
        {
            // Kiểm tra xem người chơi có nhấn đúng phím để vào ống không
            if (Input.GetKey(enterKeyCode) && other.TryGetComponent(out Player player))
            {
                StartCoroutine(Enter(player));
            }
        }
    }

   
    /// Điều khiển hiệu ứng nhân vật đi vào ống, chuyển đổi vị trí đến điểm kết nối.
    private IEnumerator Enter(Player player)
    {
        // Tắt điều khiển di chuyển của người chơi khi vào ống
        player.movement.enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.5f; // Thu nhỏ nhân vật xuống 50%
        yield return Move(player.transform, enteredPosition, enteredScale);
        yield return new WaitForSeconds(1f);

        // Kiểm tra xem nhân vật có đi xuống lòng đất không
        var sideScrolling = Camera.main.GetComponent<SideScrollingCamera>();
        sideScrolling.SetUnderground(connection.position.y < sideScrolling.undergroundThreshold);

        if (exitDirection != Vector3.zero)
        {
            player.transform.position = connection.position - exitDirection;
            yield return Move(player.transform, connection.position + exitDirection, Vector3.one);
        }
        else
        {

            player.transform.position = connection.position;
            player.transform.localScale = Vector3.one;
        }

        player.movement.enabled = true;
    }

    /// Hiệu ứng di chuyển nhân vật 
    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f; 
        float duration = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale; 

        while (elapsed < duration)
        {
            float t = elapsed / duration; 
            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }
}
