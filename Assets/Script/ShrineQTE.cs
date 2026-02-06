using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ShrineQTE : MonoBehaviour
{
    [Header("Settings")]
    public int sequenceLength = 6;
    public float timeLimit = 5.0f;

    [Header("Visual Settings")]
    public float activeScale = 1.3f;
    public float normalScale = 1.0f;
    public Color activeColor = new Color(1f, 1f, 0f, 1f);
    public Color completedColor = new Color(0f, 1f, 0f, 1f);
    public Color pendingColor = new Color(1f, 1f, 1f, 1f);

    [Header("Effect")]
    public float shakeIntensity = 5f;
    public float shakeDuration = 0.2f;

    [Header("UI References")]
    public GameObject qtePanel;
    public Transform keysContainer;
    public GameObject keyPrefab;
    public Slider timeSlider;

    private bool isEventActive = false;
    private List<KeyCode> qteSequence = new List<KeyCode>();
    private List<GameObject> spawnedKeyObjs = new List<GameObject>();
    private KeyCode[] validKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    private int currentIndex = 0;
    private float currentTimer;
    private GameObject playerRef;
    private bool wasMovementEnabled = true;

    void Start()
    {
        if (qtePanel != null) qtePanel.SetActive(false);
    }

    public void Interact(GameObject player)
    {
        if (!isEventActive) StartQTE(player);
    }

    void StartQTE(GameObject player)
    {
        isEventActive = true;
        playerRef = player;
        currentIndex = 0;
        currentTimer = timeLimit;

        if (player.GetComponent<Move>()) wasMovementEnabled = player.GetComponent<Move>().enabled;
        if (player.GetComponent<Move>()) player.GetComponent<Move>().enabled = false;
        if (player.GetComponent<CharacterController>()) player.GetComponent<CharacterController>().enabled = false;

        qtePanel.SetActive(true);
        GenerateSequence();
        UpdateKeyVisuals();
    }

    void GenerateSequence()
    {
        qteSequence.Clear();
        foreach (GameObject obj in spawnedKeyObjs) Destroy(obj);
        spawnedKeyObjs.Clear();

        KeyCode lastKey = KeyCode.None;

        for (int i = 0; i < sequenceLength; i++)
        {
            KeyCode newKey;
            do
            {
                newKey = validKeys[Random.Range(0, validKeys.Length)];
            } while (newKey == lastKey);

            qteSequence.Add(newKey);
            lastKey = newKey;

            GameObject newKeyObj = Instantiate(keyPrefab, keysContainer);

            TMP_Text tmpText = newKeyObj.GetComponentInChildren<TMP_Text>();
            if (tmpText != null) tmpText.text = newKey.ToString();
            else
            {
                Text legacyText = newKeyObj.GetComponentInChildren<Text>();
                if (legacyText != null) legacyText.text = newKey.ToString();
            }

            spawnedKeyObjs.Add(newKeyObj);
        }
    }

    void UpdateKeyVisuals()
    {
        for (int i = 0; i < spawnedKeyObjs.Count; i++)
        {
            GameObject keyObj = spawnedKeyObjs[i];
            Image bgImage = keyObj.GetComponent<Image>();
            if (bgImage == null)
            {
                bgImage = keyObj.GetComponentInChildren<Image>();
            }
            if (bgImage == null)
            {
                continue;
            }

            if (i == currentIndex)
            {
                keyObj.transform.localScale = Vector3.one * activeScale;
                bgImage.color = activeColor;
            }
            else if (i < currentIndex)
            {
                keyObj.transform.localScale = Vector3.one * normalScale;
                bgImage.color = completedColor;
            }
            else
            {
                keyObj.transform.localScale = Vector3.one * normalScale;
                bgImage.color = pendingColor;
            }
        }
    }

    void Update()
    {
        if (!isEventActive) return;

        currentTimer -= Time.deltaTime;
        if (timeSlider != null) timeSlider.value = currentTimer / timeLimit;

        if (currentTimer <= 0)
        {
            FailQTE();
            return;
        }

        CheckInput();
    }

    void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            KeyCode correctKey = qteSequence[currentIndex];
            GameObject currentObj = spawnedKeyObjs[currentIndex];

            if (Input.GetKeyDown(correctKey))
            {
                currentIndex++;
                if (currentIndex >= qteSequence.Count) WinQTE();
                else UpdateKeyVisuals();
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                     Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                Transform textTransform = null;
                if (currentObj.GetComponentInChildren<TMP_Text>())
                    textTransform = currentObj.GetComponentInChildren<TMP_Text>().transform;
                else if (currentObj.GetComponentInChildren<Text>())
                    textTransform = currentObj.GetComponentInChildren<Text>().transform;

                if (textTransform != null) StartCoroutine(ShakeUI(textTransform));
            }
        }
    }

    IEnumerator ShakeUI(Transform target)
    {
        Vector3 originalPos = target.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            target.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localPosition = originalPos;
    }

    void WinQTE()
    {
        Debug.Log("QTE Success Got x1.5 Point");
        if (spawnedKeyObjs.Count > 0)
        {
            GameObject lastKey = spawnedKeyObjs[spawnedKeyObjs.Count - 1];
            lastKey.transform.localScale = Vector3.one * normalScale;
            if (lastKey.GetComponent<Image>()) lastKey.GetComponent<Image>().color = completedColor;
        }
        StartCoroutine(CloseAfterDelay(0.5f));
    }

    void FailQTE()
    {
        Debug.Log("QTE Failed");
        EndQTE();
    }

    IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndQTE();
    }

    void EndQTE()
    {
        isEventActive = false;
        qtePanel.SetActive(false);
        foreach (GameObject obj in spawnedKeyObjs) Destroy(obj);
        spawnedKeyObjs.Clear();

        if (playerRef != null)
        {
            if (playerRef.GetComponent<Move>())
                playerRef.GetComponent<Move>().enabled = wasMovementEnabled;
            if (playerRef.GetComponent<CharacterController>())
                playerRef.GetComponent<CharacterController>().enabled = wasMovementEnabled;
        }
    }
}