using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public float obstacleLifeTime = 5f;
    public float checkRadius = 2f; 

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            TrySpawn();
            timer = 0;
        }
    }

    void TrySpawn()
    {
        if (obstaclePrefabs.Length == 0 || spawnPoints.Length == 0) return;

       
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform targetPoint = spawnPoints[randomIndex];

       
        Collider[] colliders = Physics.OverlapSphere(targetPoint.position, checkRadius);

        bool isOccupied = false;
        foreach (var col in colliders)
        {
            
            if (col.CompareTag("Obstacle"))
            {
                isOccupied = true;
                break;
            }
        }

        if (!isOccupied)
        {
            SpawnObstacle(targetPoint);
        }
        else
        {
           
            Debug.Log("จุดเกิดไม่ว่าง ข้ามการสร้างรอบนี้");
        }
    }

    void SpawnObstacle(Transform sp)
    {
        int randomPrefab = Random.Range(0, obstaclePrefabs.Length);
        GameObject newObstacle = Instantiate(obstaclePrefabs[randomPrefab], sp.position, sp.rotation);

        
        newObstacle.tag = "Obstacle";

        Destroy(newObstacle, obstacleLifeTime);
    }
}