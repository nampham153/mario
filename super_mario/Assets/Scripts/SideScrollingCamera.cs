using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrollingCamera : MonoBehaviour
{
    public Transform trackedObject; 
    public float height = 6.5f;
    public float undergroundHeight = -9.5f;
    public float undergroundThreshold = 0f; 

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position; // Lấy vị trí hiện tại của camera
        cameraPosition.x = Mathf.Max(cameraPosition.x, trackedObject.position.x);
        // Camera chỉ di chuyển theo hướng ngang nếu nhân vật di chuyển sang phải, giữ nguyên nếu nhân vật đi lùi)
        transform.position = cameraPosition; // Cập nhật lại vị trí của camera
    }

   public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position; 
        cameraPosition.y = underground ? undergroundHeight : height;
      
        transform.position = cameraPosition;
    }
}
