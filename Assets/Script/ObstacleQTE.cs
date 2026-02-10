using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEventObstacle : MonoBehaviour
{
    [Header("QTE Settings")]
    public float maxEnergy = 100f;
    public float decaySpeed = 20f;
    public float powerPerPress = 15f;

    [Header("Visual Settings")]
    public Vector3 activeScale = new Vector3(1.3f, 1.3f, 1f);
    public Vector3 inactiveScale = new Vector3(0.8f, 0.8f, 1f);

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
        if (qtePanel != null) qtePanel.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Vehicle")) && !isEventActive)
        {
            StartQTE(collision.gameObject);
        }
    }

    void StartQTE(GameObject player)
    {
        isEventActive = true;
        playerRef = player;
        currentEnergy = 0;
        isTurnQ = true;

        if (player.GetComponent<Move>()) player.GetComponent<Move>().enabled = false;
        if (player.GetComponent<CharacterController>()) player.GetComponent<CharacterController>().enabled = false;
        Motorcycle bike = player.GetComponent<Motorcycle>();
        if (bike != null)
        {
            bike.isDriving = false;
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
            WinQTE();
        }
    }

    void UpdateButtonVisuals()
    {
        if (qButtonIcon == null || eButtonIcon == null) return;

        if (isTurnQ)
        {
            qButtonIcon.localScale = activeScale;
            eButtonIcon.localScale = inactiveScale;
        }
        else
        {
            eButtonIcon.localScale = activeScale;
            qButtonIcon.localScale = inactiveScale;
        }
    }

    void UpdateUI()
    {
        float ratio = currentEnergy / maxEnergy;
        if (energySlider != null)
        {
            energySlider.value = ratio;
        }
    }

    void WinQTE()
    {
        Debug.Log("Success");
        isEventActive = false;
        if (qtePanel != null) qtePanel.SetActive(false);

        if (playerRef != null)
        {
            if (playerRef.GetComponent<Move>()) playerRef.GetComponent<Move>().enabled = true;
            if (playerRef.GetComponent<CharacterController>()) playerRef.GetComponent<CharacterController>().enabled = true;

            Motorcycle bike = playerRef.GetComponent<Motorcycle>();
            if (bike != null)
            {
                Rigidbody rb = playerRef.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = wasKinematic;
                }
                bike.isDriving = true;
            }
        }
        Destroy(gameObject);
    }
}