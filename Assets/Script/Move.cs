using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 150f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private float yVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Vertical");     // W / S
        float turnInput = Input.GetAxis("Horizontal");   // A / D

        // ===== หมุนซ้ายขวา =====
        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);

        // ===== เดินหน้า/ถอยหลัง =====
        Vector3 move = transform.forward * moveInput * moveSpeed;

        // ===== Gravity =====
        if (controller.isGrounded && yVelocity < 0)
            yVelocity = -2f;

        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);
    }
}