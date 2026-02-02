using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;  
    public Transform cam;

    CharacterController controller;
    Vector3 velocity;               
    bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
       
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0;

        Vector3 moveDir = forward * v + right * h;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (moveDir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDir),
                10f * Time.deltaTime
            );
        }

        
        velocity.y += gravity * Time.deltaTime;

        
        controller.Move(velocity * Time.deltaTime);
    }
}