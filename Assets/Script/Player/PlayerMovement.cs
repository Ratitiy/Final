using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.8f;
    public float runSpeed = 5f;
    public float rotationSpeed = 720f;

    private CharacterController controller;
    public Animator anim;

    public bool uiOpened = false;    // UI เปิด?

    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {

        if (uiOpened)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            controller.SimpleMove(Vector3.zero);
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            return;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }



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
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
