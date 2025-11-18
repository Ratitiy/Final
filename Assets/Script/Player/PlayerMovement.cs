using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.8f;
    public float runSpeed = 5f;
    public float rotationSpeed = 720f;
    private CharacterController controller;

    [Header("Detection Settings")]
    public float detectRadius = 0.5f;
    public float detectDistance = 2f;
    public LayerMask detectLayer;
    public Animator anim;

    float idleTimer = 0f;
    public float idleDelayMin = 4f;
    public float idleDelayMax = 7f;
    float nextIdleTime = 0f;

    void Start()
    {
        nextIdleTime = Random.Range(idleDelayMin, idleDelayMax);
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

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isRunning ? runSpeed : speed;
        controller.SimpleMove(move.normalized * currentSpeed);

      
        anim.SetBool("isWalking", move.magnitude > 0.1f);

        anim.SetBool("isRunning", isRunning && move.magnitude > 0.1f);





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
        
        //สุ่มท่า idle
        if (move.magnitude < 0.1f)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= nextIdleTime)
            {
                
                int randomIdle = Random.Range(1, 3);
                anim.SetInteger("idleIndex", randomIdle);

                idleTimer = 0f;
                nextIdleTime = Random.Range(idleDelayMin, idleDelayMax);
            }
        }
        else
        {
            
            anim.SetInteger("idleIndex", 0);
            idleTimer = 0f;
        }
    }

    void CheckFront()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1f;
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
