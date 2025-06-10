using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;    
    public Transform poleBottom; 
    public Transform castle;    

    public float speed = 6f;

    public int nextWorld = 1; 
    public int nextStage = 1; 


    // Xử lý khi nhân vật chạm vào cột cờ.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu nhân vật chạm vào cột cờ
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            // Di chuyển lá cờ xuống chân cột
            StartCoroutine(MoveTo(flag, poleBottom.position));

            // Bắt đầu chuỗi sự kiện hoàn thành màn chơi
            StartCoroutine(LevelCompleteSequence(player));
        }
    }

    // Chuỗi sự kiện khi hoàn thành màn chơi.
    private IEnumerator LevelCompleteSequence(Player player)
    {
        // Vô hiệu hóa di chuyển của nhân vật
        player.movement.enabled = false;

        // Di chuyển nhân vật đến chân cột cờ
        yield return MoveTo(player.transform, poleBottom.position);

        // Nhân vật di chuyển sang phải
        yield return MoveTo(player.transform, player.transform.position + Vector3.right);

        // Nhân vật di chuyển xuống 
        yield return MoveTo(player.transform, player.transform.position + Vector3.right + Vector3.down);

        // Nhân vật di chuyển vào lâu đài
        yield return MoveTo(player.transform, castle.position);

        // Ẩn nhân vật sau khi vào lâu đài
        player.gameObject.SetActive(false);

        // Đợi 2 giây trước khi tải màn chơi tiếp theo
        yield return new WaitForSeconds(2f);

        // Tải màn chơi tiếp theo dựa trên giá trị nextWorld và nextStage
        GameManager.Instance.LoadLevel(nextWorld, nextStage);
    }

    // Di chuyển một đối tượng đến vị trí đích với tốc độ đã cho.
    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }
        subject.position = position;
    }
}
