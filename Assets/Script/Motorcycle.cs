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
    public float breakTorque = 3000f;
    public float steeringAngle = 25f;
    public float centerOfMassOffset = -0.7f;
    public float maxSpeedKmh = 80f;

    [Header("Stability Settings")]
    public float stabilitySmoothness = 10f;
    public float tiltAmount = 20f;

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

        if (currentSpeedKmh < maxSpeedKmh)
        {
            backWheel.motorTorque = moveInput * motorTorque;
        }
        else
        {
            backWheel.motorTorque = 0;
        }
    }

    void ApplyStability()
    {
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        if (Mathf.Abs(steerInput) > 0.1f)
        {
            float leanAngle = -steerInput * tiltAmount;
            targetRotation *= Quaternion.Euler(0, 0, leanAngle);
        }

        Quaternion predictedRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * stabilitySmoothness);
        rb.MoveRotation(predictedRotation);

        if (Mathf.Abs(steerInput) > 0.1f)
        {
            float turnHelp = steerInput * 50f * rb.linearVelocity.magnitude;
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
    public void Interact(GameObject player)
    {
        this.enabled = true;

        if (player.GetComponent<Move>()) player.GetComponent<Move>().enabled = false;
        if (player.GetComponent<CharacterController>()) player.GetComponent<CharacterController>().enabled = false;
    }
}
