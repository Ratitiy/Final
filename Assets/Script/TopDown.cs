using UnityEngine;

public class TopDown : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 15, -5);
    public float smoothSpeed = 10f;
    public float rotationSmooth = 5f; 

    void LateUpdate()
    {
        if (target == null) return;

       
        Vector3 desiredPosition = target.TransformPoint(offset);

        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);
    }
}