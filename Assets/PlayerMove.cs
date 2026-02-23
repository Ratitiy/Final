using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    Rigidbody rb;
    float horizontal;
    float vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // กันตัวล้ม
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        Vector3 velocity = moveDirection.normalized * moveSpeed;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        // หมุนตามทิศที่เดิน
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}