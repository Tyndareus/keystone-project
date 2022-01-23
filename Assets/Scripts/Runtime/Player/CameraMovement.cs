using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float xMinBorder;
    [SerializeField] private float xMaxBorder;
    [SerializeField] private float yMinBorder;
    [SerializeField] private float yMaxBorder;
    
    private void LateUpdate()
    {
        Vector3 camPos = transform.position;
        
        Vector3 playerPos = playerTransform.position;
        float xPos = playerPos.x;
        xPos = Mathf.Clamp(xPos, xMinBorder, xMaxBorder);

        float yPos = playerPos.y;
        yPos = Mathf.Clamp(yPos, yMinBorder, yMaxBorder);

        transform.position = new Vector3(xPos, yPos, camPos.z);
    }
}