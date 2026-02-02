using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          
    public float smoothSpeed = 5f;    

    [Header("Settings")]
    public Vector3 offset;            
    public bool isTopDown = false;    

    void LateUpdate()
    {
        if (target == null) return;

        
        Vector3 desiredPosition = target.position + offset;

        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        
        if (isTopDown)
        {
            
            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90f, target.eulerAngles.y, 0f), smoothSpeed * Time.deltaTime);
        }
        else
        {
            
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }

    
    public void SetTarget(Transform newTarget, Vector3 newOffset, bool topDown)
    {
        target = newTarget;
        offset = newOffset;
        isTopDown = topDown;
    }
}