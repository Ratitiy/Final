using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEventObstacle : MonoBehaviour
{
    [Header("QTE Settings")]
    public float maxEnergy = 100f;
    public float decaySpeed = 20f;
    public float powerPerPress = 10f;
    public bool canTriggerQTE = true;

    [Header("QTE Rules")]
    public float qteTimeLimit = 5f;
    private float currentQteTimer;

    [Header("Cooldown Settings")]
    public float cooldownTime = 5f;
    public static bool isGlobalQTEActive = false;
    public static float globalCooldownEndTime = 0f;

    [Header("Visual Settings (Scale)")]
    public Vector3 activeScale = new Vector3(1.3f, 1.3f, 1f);
    public Vector3 inactiveScale = new Vector3(0.8f, 0.8f, 1f);
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    [Header("UI References")]
    public GameObject qtePanel;
    public Slider energySlider;
    public Transform qButtonIcon;
    public Transform eButtonIcon;

    private float currentEnergy;
    private bool isEventActive = false;
    private GameObject playerRef;
    private bool isTurnQ = true;
    private bool wasKinematic;

    void Start()
    {
        if (!isGlobalQTEActive && qtePanel != null && qtePanel.activeSelf)
        {
            qtePanel.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!canTriggerQTE || isGlobalQTEActive || Time.time < globalCooldownEndTime) return;

        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Vehicle")) && !isEventActive)
        {
            StartQTE(collision.gameObject);
        }
    }

    void StartQTE(GameObject player)
    {
        isEventActive = true;
        isGlobalQTEActive = true;
        playerRef = player;
        currentEnergy = 0;
        currentQteTimer = qteTimeLimit;
        isTurnQ = true;

        RatMove moveScript = GetComponent<RatMove>();
        if (moveScript != null) moveScript.enabled = false;

        Rigidbody myRb = GetComponent<Rigidbody>();
        if (myRb != null) myRb.isKinematic = true;

        if (player.GetComponent<PlayerMovement>()) player.GetComponent<PlayerMovement>().enabled = false;
        if (player.GetComponent<CharacterController>()) player.GetComponent<CharacterController>().enabled = false;

        MotocycleV2 bike = player.GetComponent<MotocycleV2>();
        if (bike != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                wasKinematic = rb.isKinematic;
                rb.isKinematic = true;
            }
        }

        if (qtePanel != null) qtePanel.SetActive(true);
        UpdateButtonVisuals();
    }

    void Update()
    {
        if (!isEventActive) return;

        currentQteTimer -= Time.deltaTime;
        if (currentQteTimer <= 0)
        {
            EndQTE(false);
            return;
        }

        if (isTurnQ)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentEnergy += powerPerPress;
                isTurnQ = false;
                UpdateButtonVisuals();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentEnergy += powerPerPress;
                isTurnQ = true;
                UpdateButtonVisuals();
            }
        }

        currentEnergy -= decaySpeed * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        UpdateUI();

        if (currentEnergy >= maxEnergy)
        {
            EndQTE(true);
        }
    }

    void EndQTE(bool isWin)
    {
        isEventActive = false;
        isGlobalQTEActive = false;
        globalCooldownEndTime = Time.time + cooldownTime;

        if (qtePanel != null) qtePanel.SetActive(false);

        if (playerRef != null)
        {
            if (playerRef.GetComponent<PlayerMovement>()) playerRef.GetComponent<PlayerMovement>().enabled = true;
            if (playerRef.GetComponent<CharacterController>()) playerRef.GetComponent<CharacterController>().enabled = true;

            MotocycleV2 bike = playerRef.GetComponent<MotocycleV2>();
            if (bike != null)
            {
                Rigidbody rb = playerRef.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = wasKinematic;
                bike.enabled = true;
            }
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        RatMove moveScript = GetComponent<RatMove>();
        if (moveScript != null) moveScript.enabled = true;

        Rigidbody myRb = GetComponent<Rigidbody>();
        if (myRb != null) myRb.isKinematic = false;
    }

    void OnDestroy()
    {
        if (isEventActive)
        {
            isGlobalQTEActive = false;
            if (qtePanel != null) qtePanel.SetActive(false);

            if (playerRef != null)
            {
                if (playerRef.GetComponent<PlayerMovement>()) playerRef.GetComponent<PlayerMovement>().enabled = true;
                if (playerRef.GetComponent<CharacterController>()) playerRef.GetComponent<CharacterController>().enabled = true;
                MotocycleV2 bike = playerRef.GetComponent<MotocycleV2>();
                if (bike != null)
                {
                    Rigidbody rb = playerRef.GetComponent<Rigidbody>();
                    if (rb != null) rb.isKinematic = wasKinematic;
                    bike.enabled = true;
                }
            }
        }
    }

    void UpdateButtonVisuals()
    {
        if (qButtonIcon == null || eButtonIcon == null) return;
        Image qImage = qButtonIcon.GetComponent<Image>();
        Image eImage = eButtonIcon.GetComponent<Image>();
        if (isTurnQ)
        {
            qButtonIcon.localScale = activeScale;
            eButtonIcon.localScale = inactiveScale;
            if (qImage != null) qImage.color = activeColor;
            if (eImage != null) eImage.color = inactiveColor;
        }
        else
        {
            eButtonIcon.localScale = activeScale;
            qButtonIcon.localScale = inactiveScale;
            if (eImage != null) eImage.color = activeColor;
            if (qImage != null) qImage.color = inactiveColor;
        }
    }

    void UpdateUI()
    {
        float ratio = currentEnergy / maxEnergy;
        if (energySlider != null) energySlider.value = ratio;
    }
}