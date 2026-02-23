using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 12f;          
    public float gravity = -9.81f;     

    [Header("Ground Check Settings")]
    public Transform groundCheck;      
    public float groundDistance = 0.4f;
    public LayerMask groundMask;      

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
       
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");   

       
        Vector3 move = transform.right * x + transform.forward * z;

        // สั่งให้ตัวละครเดิน
        controller.Move(move * speed * Time.deltaTime);

        // 3. คำนวณแรงโน้มถ่วงดึงตัวละครลงพื้น
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}