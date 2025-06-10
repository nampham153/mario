using UnityEngine;

public static class Extensions
{

    private static LayerMask layerMask = LayerMask.GetMask("Default");

    // Kiểm tra xem Rigidbody2D có bị chặn đường bởi một vật thể khác theo hướng chỉ định hay không.
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        // Nếu Rigidbody2D là kinematic (không bị ảnh hưởng bởi vậ
        if (rigidbody.isKinematic)
        {
            return false;
        }

        // Xác định bán kính của vòng tròn kiểm tra va chạm.
        float radius = 0.25f;
        // Xác định khoảng cách tối đa để kiểm tra va chạm.
        float distance = 0.375f;

        RaycastHit2D hit = Physics2D.CircleCast(
            rigidbody.position,  
            radius,             
            direction.normalized, 
            distance,        
            layerMask           
        );

        // Trả về true nếu có va chạm với một vật thể khác 
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        // Tính vector hướng từ transform hiện tại đến đối tượng khác.
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}
