using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathBarrier : MonoBehaviour
{
    //gọi khi một đối tượng có Collider2D chạm vào BoxCollider2D của DeathBarrier 
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm có tag "Player" 
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false); // Ẩn nhân vật 
            GameManager.Instance.ResetLevel(3f);
        }
        else
        {
            Destroy(other.gameObject); 
        }
    }
}
