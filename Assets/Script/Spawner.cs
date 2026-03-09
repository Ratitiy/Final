using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] obstaclePrefabs; 
    public Transform[] spawnPoints;      

    public float spawnInterval = 3f;     
    public bool isSpawning = true;

    private float timer;

    void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0 || spawnPoints.Length == 0) return;

        
        int randomPoint = Random.Range(0, spawnPoints.Length);
        Transform sp = spawnPoints[randomPoint];

       
        int randomPrefab = Random.Range(0, obstaclePrefabs.Length);

       
        Instantiate(obstaclePrefabs[randomPrefab], sp.position, sp.rotation);
    }
}