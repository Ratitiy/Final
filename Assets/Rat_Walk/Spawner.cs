using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject prefabToSpawn;
    public Transform targetPoint;
    public int ratsPerWave = 3;
    public float spawnInterval = 0.3f;
    public float waveCooldown = 5f;
    public float distanceBetweenRats = 0.8f;
    public float delayBeforeQTEActive = 1.0f;

    [Header("QTE UI For Spawned Rats")]
    public GameObject globalQtePanel;
    public Slider globalEnergySlider;
    public Transform globalQButton;
    public Transform globalEButton;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            for (int i = 0; i < ratsPerWave; i++)
            {
                SpawnRat(i);
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return new WaitForSeconds(waveCooldown);
        }
    }

    void SpawnRat(int index)
    {
        if (prefabToSpawn == null || targetPoint == null) return;

        GameObject newRat = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

        RatMove moveScript = newRat.GetComponent<RatMove>();
        if (moveScript != null)
        {
            moveScript.target = targetPoint;
            Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;
            moveScript.offset = -directionToTarget * (index * distanceBetweenRats);
        }

        QuickTimeEventObstacle qteScript = newRat.GetComponent<QuickTimeEventObstacle>();
        if (qteScript != null)
        {
            qteScript.qtePanel = globalQtePanel;
            qteScript.energySlider = globalEnergySlider;
            qteScript.qButtonIcon = globalQButton;
            qteScript.eButtonIcon = globalEButton;

            qteScript.canTriggerQTE = false;
            StartCoroutine(ActivateQTEAfterDelay(qteScript, delayBeforeQTEActive));
        }
    }

    IEnumerator ActivateQTEAfterDelay(QuickTimeEventObstacle qte, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (qte != null)
        {
            qte.canTriggerQTE = true;
        }
    }
}