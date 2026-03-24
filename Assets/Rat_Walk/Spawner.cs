using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject prefabToSpawn;
    public Transform targetPoint;
    public int ratsPerWave = 3;        
    public float spawnInterval = 0.3f;   
    public float waveCooldown = 5f;      
    public float distanceBetweenRats = 0.8f;

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
    }
}