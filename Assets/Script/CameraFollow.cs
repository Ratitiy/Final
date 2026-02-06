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

        Vector3 desiredPosition;

        if (isTopDown)
        {
            desiredPosition = target.position + offset;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90f, target.eulerAngles.y, 0f), smoothSpeed * Time.deltaTime);
        }
        else
        {
            desiredPosition = target.TransformPoint(offset);
            var lookAtTarget = target.position + (Vector3.up * 1.5f);
            Vector3 direction = lookAtTarget - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, smoothSpeed * Time.deltaTime);
            }
        }
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget, Vector3 newOffset, bool topDown)
    {
        target = newTarget;
        offset = newOffset;
        isTopDown = topDown;
    }
}