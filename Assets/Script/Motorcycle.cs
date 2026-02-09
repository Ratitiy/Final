using UnityEngine;

public class Motorcycle : MonoBehaviour
{
    [Header("Car Settings")]
    public float speed = 1500f;       
    public float turnSpeed = 50f;     

    [Header("Status")]
    public bool isDriving = false;   

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isDriving) return;
        float moveInput = Input.GetAxis("Vertical");    
        float turnInput = Input.GetAxis("Horizontal");  
        MoveCar(moveInput, turnInput);
    }

    void MoveCar(float move, float turn)
    {
        if (move != 0)
        {
            rb.AddForce(transform.forward * move * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        if (turn != 0)
        {
            Vector3 rotation = Vector3.up * turn * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }
    }
    public void Interact(GameObject player)
    {
        if (isDriving) return;
        isDriving = true;

        if (player.GetComponent<Move>()) player.GetComponent<Move>().enabled = false;
        if (player.GetComponent<CharacterController>()) player.GetComponent<CharacterController>().enabled = false;
    }
}