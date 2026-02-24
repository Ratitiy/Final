using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.8f;
    public float runSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    Animator animator;

    private CharacterController controller;
    private float rotationVelocity;

    public bool uiOpened = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (controller == null || !controller.enabled)
            return;

        if (Time.timeScale == 0f) return;

        if (uiOpened)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            controller.SimpleMove(Vector3.zero);
            animator.SetFloat("Speed", 0f);
            return;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        if (!animator.GetBool("IsRiding"))
        {
            animator.SetFloat("Speed", inputDir.magnitude, 0.1f, Time.deltaTime);
        }

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg
                                + Camera.main.transform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref rotationVelocity,
                rotationSmoothTime
            );

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : speed;

            controller.SimpleMove(moveDir * currentSpeed);
        }
        else
        {
            controller.SimpleMove(Vector3.zero);
        }
    }
}