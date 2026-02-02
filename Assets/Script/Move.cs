using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f;
    public Transform cam;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
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
    }
}
