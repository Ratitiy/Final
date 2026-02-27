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
    private Transform camTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
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

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        if (!animator.GetBool("IsRiding"))
        {
            animator.SetFloat("Speed", inputDir.magnitude, 0.1f, Time.deltaTime);
        }

        if (inputDir.magnitude >= 0.1f)
        {
            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (camForward * vertical) + (camRight * horizontal);

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : speed;
            controller.SimpleMove(moveDir.normalized * currentSpeed);
        }
        else
        {
            controller.SimpleMove(Vector3.zero);
        }
    }
}