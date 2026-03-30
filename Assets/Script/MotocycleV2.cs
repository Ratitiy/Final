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

    [Header("Debuff Settings")]
    public bool isControlsInverted = false;
    public float slipDelay = 0.3f;

    [Header("Debuff UI")]
    public GameObject debuffPanel;
    public UnityEngine.UI.Image waterSplatterImage; 
    public UnityEngine.UI.Image timerCircleImage;   

    [Header("Settings")]
    public float motorTorque = 500f;
    public float breakTorque = 3000f;
    public float steeringAngle = 15f;
    public float centerOfMassOffset = -0.7f;
    public float maxSpeedKmh = 20f;
    [Header("Upgrade Limits")]
    public float minSteering = 15f;
    public float maxSteering = 25f;
    [Header("Stability Settings")]
    public float stabilitySmoothness = 10f;
    public float tiltAmount = 20f;
    public LayerMask groundLayer = ~0;

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
        float rawVertical = Input.GetAxis("Vertical");     
        float rawHorizontal = Input.GetAxis("Horizontal"); 
        if (isControlsInverted)
        {
            moveInput = -rawHorizontal;
            steerInput = rawVertical; 
        }
        else
        {
            moveInput = rawVertical;
            steerInput = rawHorizontal;
        }

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
        backWheel.motorTorque = moveInput * motorTorque;
        if (currentSpeedKmh > maxSpeedKmh)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (maxSpeedKmh / 3.6f);
        }
    }


    void ApplyStability()
    {
        Quaternion targetRotation;
        RaycastHit hit;

        
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

        
        float leanAngle = -steerInput * tiltAmount;

       
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

    public void TriggerSlipEffect(float duration)
    {
        StartCoroutine(SlipRoutine(duration));
    }

    private System.Collections.IEnumerator SlipRoutine(float duration)
    {
        yield return new WaitForSeconds(slipDelay);
        isControlsInverted = true;
        if (debuffPanel != null) debuffPanel.SetActive(true);

        if (waterSplatterImage != null)
        {
            Color c = waterSplatterImage.color;
            c.a = 1f;
            waterSplatterImage.color = c;
        }

        if (timerCircleImage != null)
        {
            timerCircleImage.fillAmount = 1f; 
        }

        float timeLeft = duration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float ratio = timeLeft / duration;

            if (waterSplatterImage != null)
            {
                Color c = waterSplatterImage.color;
                c.a = ratio;
                waterSplatterImage.color = c;
            }
            if (timerCircleImage != null)
            {
                timerCircleImage.fillAmount = ratio;
            }

            yield return null;
        }
        isControlsInverted = false;
        if (debuffPanel != null) debuffPanel.SetActive(false);
    }
   
    public void UpgradeMaxSpeed(float amount)
    {
        maxSpeedKmh += amount;
    }

    public void UpgradeAcceleration(float amount)
    {
        motorTorque += amount; 
    }

    public void UpgradeSteering(float amount)
    {
        steeringAngle += amount;
       
        steeringAngle = Mathf.Clamp(steeringAngle, 15f, 25f);
    }
}