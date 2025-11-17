using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCarController : MonoBehaviour
{
    public float acceleration = 12f;
    public float reverseAcceleration = 8f;
    public float maxSpeed = 25f;
    public float steerAngle = 80f;
    public float dragWhenNoInput = 2f;

    Rigidbody rb;
    bool controlEnabled;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void Update()
    {
        if (!controlEnabled) return;

        float steer = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, steer * steerAngle * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (!controlEnabled) return;

        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        float speed = Vector3.Dot(rb.linearVelocity, forward);

        float accel = v > 0 ? acceleration : reverseAcceleration;
        Vector3 force = forward * v * accel;

        if (v > 0 && speed > maxSpeed) force = Vector3.zero;

        rb.AddForce(force, ForceMode.Acceleration);

        if (Mathf.Approximately(v, 0f))
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, dragWhenNoInput * Time.fixedDeltaTime);
    }

    public void SetControlEnabled(bool enabled)
    {
        controlEnabled = enabled;
        if (!enabled) rb.angularVelocity = Vector3.zero;
    }
}
