using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Motorcycle : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontWheel;
    public WheelCollider backWheel;

    [Header("Wheel Transforms")]
    public Transform frontWheelMesh;
    public Transform backWheelMesh;
    public Transform frontMudguard;

    [Header("UI Settings")]
    public TextMeshProUGUI speedText;

    [Header("Settings")]
    public float motorTorque = 1500f;
    public float brakeTorque = 3000f;
    public float steeringAngle = 25f;
    public float centerOfMassOffset = -0.7f;
    public float maxSpeedKmh = 80f;

    [Header("Stability Settings")]
    public float tiltAmount = 15f;          // ความแรงการเอน
    public float stabilityForce = 5f;       // กันล้มแบบนุ่ม ๆ

    [HideInInspector] public Rigidbody rb;
    private float moveInput;
    private float steerInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.centerOfMass = new Vector3(0, centerOfMassOffset, -0.5f);

        rb.linearDamping = 0.2f;
        rb.angularDamping = 2f;

        this.enabled = false; // เปิดเมื่อขึ้นรถ
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

        DisplaySpeed();

        if (Input.GetKey(KeyCode.Space))
            ApplyBrakes(brakeTorque);
        else
            ApplyBrakes(0);
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        ApplyStability();
        UpdateWheelVisuals();
    }

    void HandleMotor()
    {
        float currentSpeedKmh = rb.linearVelocity.magnitude * 3.6f;

        if (currentSpeedKmh < maxSpeedKmh)
            backWheel.motorTorque = moveInput * motorTorque;
        else
            backWheel.motorTorque = 0;
    }

    void HandleSteering()
    {
        frontWheel.steerAngle = steerInput * steeringAngle;
    }

    void ApplyBrakes(float force)
    {
        frontWheel.brakeTorque = force;
        backWheel.brakeTorque = force;
    }

    // 🔥 ระบบเอนแบบใช้แรงจริง ไม่ใช้ MoveRotation แล้ว
    void ApplyStability()
    {
        float speed = rb.linearVelocity.magnitude;

        // เอนตามการเลี้ยว
        if (Mathf.Abs(steerInput) > 0.1f && speed > 1f)
        {
            float leanTorque = -steerInput * tiltAmount * 50f;
            rb.AddRelativeTorque(Vector3.forward * leanTorque);
        }

        // กันล้มแบบนุ่ม ๆ (ไม่ดีด)
        float uprightTorque = -transform.right.x * stabilityForce;
        rb.AddRelativeTorque(Vector3.forward * uprightTorque);
    }

    void DisplaySpeed()
    {
        if (speedText != null)
        {
            float currentSpeedKmh = rb.linearVelocity.magnitude * 3.6f;
            speedText.text = currentSpeedKmh.ToString("0") + " KM/H";
        }
    }

    void UpdateWheelVisuals()
    {
        UpdateSingleWheel(frontWheel, frontWheelMesh);
        UpdateSingleWheel(backWheel, backWheelMesh);

        if (frontMudguard != null)
        {
            Vector3 pos; Quaternion rot;
            frontWheel.GetWorldPose(out pos, out rot);

            frontMudguard.position = pos;

            Vector3 euler = rot.eulerAngles;
            frontMudguard.rotation = Quaternion.Euler(transform.eulerAngles.x, euler.y, transform.eulerAngles.z);
        }
    }

    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        if (wheelTransform == null) return;

        Vector3 pos; Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    void OnDisable()
    {
        moveInput = 0;
        steerInput = 0;

        if (frontWheel != null)
        {
            frontWheel.motorTorque = 0;
            frontWheel.steerAngle = 0;
            frontWheel.brakeTorque = brakeTorque;
        }

        if (backWheel != null)
        {
            backWheel.motorTorque = 0;
            backWheel.brakeTorque = brakeTorque;
        }
    }
}