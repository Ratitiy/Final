using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class MotocycleV2 : MonoBehaviour
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
    public float breakTorque = 3000f;
    public float steeringAngle = 25f;
    public float centerOfMassOffset = -0.7f;
    public float maxSpeedKmh = 40f;

    [Header("Stability Settings")]
    public float stabilitySmoothness = 10f;
    public float tiltAmount = 20f;
    public LayerMask groundLayer = ~0; // เพิ่ม LayerMask เพื่อให้ Raycast โดนเฉพาะพื้น (ค่า default คือทุกอย่าง)

    [HideInInspector] public Rigidbody rb;
    private float moveInput;
    private float steerInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, centerOfMassOffset, -0.5f);
        this.enabled = false;
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

        // แสดงผลความเร็วบน UI
        DisplaySpeed();

        if (Input.GetKey(KeyCode.Space))
            ApplyBrakes(breakTorque);
        else
            ApplyBrakes(0);
    }

    void DisplaySpeed()
    {
        if (speedText != null)
        {
            float currentSpeedKmh = rb.linearVelocity.magnitude * 3.6f;
            speedText.text = currentSpeedKmh.ToString("0") + " KM/H";
        }
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheelVisuals();
        ApplyStability();
    }

    void HandleMotor()
    {
        float currentSpeedKmh = rb.linearVelocity.magnitude * 3.6f;

        // ใส่แรงเครื่องปกติ
        backWheel.motorTorque = moveInput * motorTorque;

        // จำกัดความเร็วจริง ๆ
        if (currentSpeedKmh > maxSpeedKmh)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (maxSpeedKmh / 3.6f);
        }
    }


    void ApplyStability()
    {
        Quaternion targetRotation;
        RaycastHit hit;

        // ใช้ forward ปัจจุบันจริง ๆ (ไม่ตัดแกน Z ทิ้ง)
        Vector3 currentForward = transform.forward;

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 3.0f, groundLayer))
        {
            Vector3 projectedForward = Vector3.ProjectOnPlane(currentForward, hit.normal);
            targetRotation = Quaternion.LookRotation(projectedForward, hit.normal);
        }
        else
        {
            targetRotation = transform.rotation;
        }

        // Lean ตอนเลี้ยว (แต่ไม่บังคับ 0 เมื่อปล่อยปุ่ม)
        float leanAngle = -steerInput * tiltAmount;

        // เพิ่มการคืนตัวแบบค่อยเป็นค่อยไป
        float autoBalance = Mathf.Lerp(transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z,
                                       leanAngle,
                                       Time.fixedDeltaTime * stabilitySmoothness);

        targetRotation = Quaternion.Euler(
            targetRotation.eulerAngles.x,
            targetRotation.eulerAngles.y,
            autoBalance
        );

        rb.MoveRotation(Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.fixedDeltaTime * stabilitySmoothness
        ));

        // ช่วยเลี้ยว
        if (Mathf.Abs(steerInput) > 0.1f)
        {
            float turnHelp = steerInput * 40f * rb.linearVelocity.magnitude;
            rb.AddRelativeTorque(Vector3.up * turnHelp);
        }
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

    void OnDisable()
    {
        moveInput = 0; steerInput = 0;
        if (frontWheel != null) { frontWheel.motorTorque = 0; frontWheel.steerAngle = 0; frontWheel.brakeTorque = breakTorque; }
        if (backWheel != null) { backWheel.motorTorque = 0; backWheel.brakeTorque = breakTorque; }
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
}