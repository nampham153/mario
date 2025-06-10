using UnityEngine;

public class PowerUp : MonoBehaviour
{
 
    public enum Type
    {
        Coin,          
        ExtraLife,      
        MagicMushroom, 
        Starpower      
    }
    public Type type; // Loại power-up cụ thể của vật thể này

    // Xử lý sự kiện khi có đối tượng khác va chạm với PowerUp.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm là "Player" 
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Collect(player); // Gọi hàm xử lý thu thập power-up
        }
    }

    // Xử lý hành động khi nhân vật thu thập power-up.
    private void Collect(Player player)
    {
        // Dựa trên loại power-up, thực hiện hành động tương ứng
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin(); // Tăng số xu
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife(); // Thêm mạng sống
                break;

            case Type.MagicMushroom:
                player.Grow(); // Nhân vật lớn hơn
                break;

            case Type.Starpower:
                player.Starpower(); // Nhân vật trở nên bất tử trong một khoảng thời gian
                break;
        }

        // Sau khi thu thập, hủy đối tượng PowerUp
        Destroy(gameObject);
    }
}
