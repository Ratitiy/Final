using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 720f;
    private CharacterController controller;

    [Header("Detection Settings")]
    public float detectRadius = 0.5f;
    public float detectDistance = 2f;
    public LayerMask detectLayer;
    public Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical);
        move = Camera.main.transform.TransformDirection(move);
        move.y = 0f;

        controller.SimpleMove(move.normalized * speed);

        if (move.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        CheckFront();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void CheckFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0f;
        Vector3 direction = transform.forward;
        bool isHit = Physics.SphereCast(origin, detectRadius, direction, out hit, detectDistance, detectLayer);
        Color rayColor = isHit ? Color.red : Color.green;
        Debug.DrawRay(origin, direction * detectDistance, rayColor);
        if (isHit)
        {
            Debug.DrawLine(origin, hit.point, Color.yellow);
            Debug.Log("เจอวัตถุด้านหน้า: " + hit.collider.name);
        }
    }
}
