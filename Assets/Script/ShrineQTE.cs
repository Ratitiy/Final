using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShrineQTE : MonoBehaviour
{
    [Header("QTE Settings")]
    public float timeLimit = 5f;
    public int sequenceLength = 4;

    [Header("UI References")]
    public GameObject qtePanel;
    public Slider timerSlider;

    [Header("Sequence UI")]
    public Image[] sequenceIconSlots;
    public GameObject[] checkmarkIcons;

    [Header("WASD Sprites")]
    public Sprite spriteW;
    public Sprite spriteA;
    public Sprite spriteS;
    public Sprite spriteD;

    private float currentTime;
    private bool isEventActive = false;
    private bool hasPlayed = false;

    private List<KeyCode> currentSequence = new List<KeyCode>();
    private int currentStep = 0;

    private GameObject playerRef;
    private RigidbodyConstraints originalConstraints;

    void Start()
    {
        if (qtePanel != null) qtePanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasPlayed || isEventActive) return;

        if (other.CompareTag("Player") || other.CompareTag("Vehicle"))
        {
            StartQTE(other.gameObject);
        }
    }

    void StartQTE(GameObject player)
    {
        isEventActive = true;
        playerRef = player;
        currentTime = timeLimit;
        currentStep = 0;

        FreezePlayer(true);
        GenerateSequence();

        if (qtePanel != null) qtePanel.SetActive(true);
    }

    void GenerateSequence()
    {
        currentSequence.Clear();
        KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        Sprite[] sprites = { spriteW, spriteA, spriteS, spriteD };

        int lastRandomIndex = -1;

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, 4);
            while (randomIndex == lastRandomIndex)
            {
                randomIndex = Random.Range(0, 4);
            }
            lastRandomIndex = randomIndex;

            currentSequence.Add(keys[randomIndex]);

            if (i < sequenceIconSlots.Length) sequenceIconSlots[i].sprite = sprites[randomIndex];
            if (i < checkmarkIcons.Length) checkmarkIcons[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!isEventActive) return;

        currentTime -= Time.deltaTime;
        if (timerSlider != null) timerSlider.value = currentTime / timeLimit;

        if (currentTime <= 0)
        {
            LoseQTE();
            return;
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(currentSequence[currentStep]))
            {
                if (currentStep < checkmarkIcons.Length) checkmarkIcons[currentStep].SetActive(true);
                currentStep++;

                if (currentStep >= sequenceLength) WinQTE();
            }
        }
    }

    void WinQTE()
    {
        Debug.Log("QTE Success! ได้บัฟคูณคะแนนแล้ว!");
        EndQTE();
    }

    void LoseQTE()
    {
        Debug.Log("QTE Failed! อดได้บัฟ");
        EndQTE();
    }

    void EndQTE()
    {
        isEventActive = false;
        hasPlayed = true;

        if (qtePanel != null) qtePanel.SetActive(false);
        FreezePlayer(false);
    }

    void FreezePlayer(bool freeze)
    {
        if (playerRef == null) return;

        MotocycleV2 bike = playerRef.GetComponent<MotocycleV2>();
        if (bike != null) bike.enabled = !freeze;

        PlayerMovement person = playerRef.GetComponent<PlayerMovement>();
        if (person != null) person.enabled = !freeze;

        Rigidbody rb = playerRef.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (freeze)
            {
                originalConstraints = rb.constraints;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                rb.constraints = originalConstraints;
            }
        }
    }
}