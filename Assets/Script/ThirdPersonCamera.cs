using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float mouseSensitivity = 200f;
    public float distance = 4f;
    public float minY = -30f;
    public float maxY = 60f;
    public float heightOffset = 1.6f;  

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 angles = transform.eulerAngles;
        yRotation = angles.y;
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (target == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

       
        Vector3 targetPosition = target.position + Vector3.up * heightOffset;

        Vector3 position = targetPosition - (rotation * Vector3.forward * distance);

        transform.position = position;
        transform.rotation = rotation;
    }
}