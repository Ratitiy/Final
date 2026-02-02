using UnityEngine;

public class Motorcycle : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 800f;
    public float turnSpeed = 80f;

    [Header("Ground Align")]
    public float alignSpeed = 6f;
    public float groundCheckDistance = 1.8f;
    public LayerMask groundLayer;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enabled = false;
    }

    void Start()
    {
        
        rb.centerOfMass = new Vector3(0, -0.8f, 0);

        
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        AlignToGround();
    }

    

    void Move()
    {
        float v = Input.GetAxis("Vertical");
        rb.AddForce(transform.forward * v * speed * Time.fixedDeltaTime, ForceMode.Force);
    }

    void Turn()
    {
        float h = Input.GetAxis("Horizontal");
        Quaternion turn =
            Quaternion.Euler(0, h * turnSpeed * Time.fixedDeltaTime, 0);

        rb.MoveRotation(rb.rotation * turn);
    }



    void AlignToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.6f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            Quaternion targetRotation =
                Quaternion.FromToRotation(transform.up, hit.normal) * rb.rotation;

            Quaternion smoothRotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                alignSpeed * Time.fixedDeltaTime
            );

            rb.MoveRotation(smoothRotation);
        }

        Debug.DrawRay(
            transform.position + Vector3.up * 0.6f,
            Vector3.down * groundCheckDistance,
            Color.yellow
        );
    }

}
