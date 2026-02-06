using UnityEngine;
using UnityEngine.UI;

public class ObstacleQTE : MonoBehaviour
{
    [Header("QTE Settings")]
    public float maxEnergy = 100f;
    public float decaySpeed = 30f;
    public float powerPerPress = 15f;

    [Header("UI References")]
    public GameObject qtePanel;
    public Slider energySlider;

    private bool isEventActive = false;
    private float currentEnergy = 0f;
    private MonoBehaviour pausedMovementScript;

    void Start()
    {
        if (qtePanel != null) qtePanel.SetActive(false);
    }

    void Update()
    {
        if (!isEventActive) return;
        currentEnergy -= decaySpeed * Time.deltaTime;

        if (currentEnergy < 0) currentEnergy = 0;

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            currentEnergy += powerPerPress;
            // Add Effect or Sound Animation Here !!
        }

        if (energySlider != null)
        {
            energySlider.value = currentEnergy / maxEnergy;
        }
        if (currentEnergy >= maxEnergy)
        {
            EndQTE();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isEventActive)
        {
            StartQTE(collision.gameObject);
        }
    }

    void StartQTE(GameObject playerObj)
    {
        isEventActive = true;
        currentEnergy = 0;

        if (qtePanel != null) qtePanel.SetActive(true);

        Motorcycle bike = playerObj.GetComponentInParent<Motorcycle>();
        if (bike != null && bike.enabled)
        {
            pausedMovementScript = bike;
            if (bike.rb != null)
            {
                bike.rb.linearVelocity = Vector3.zero;
            }
        }
        else
        {
            pausedMovementScript = playerObj.GetComponent<Move>();
        }

        if (pausedMovementScript != null) pausedMovementScript.enabled = false;
    }

    void EndQTE()
    {
        Debug.Log("Came loose!");
        isEventActive = false;
        if (qtePanel != null) qtePanel.SetActive(false);
        if (pausedMovementScript != null) pausedMovementScript.enabled = true;
        Destroy(gameObject);
        GetComponent<Collider>().enabled = false;
    }
}