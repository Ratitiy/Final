using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; 
    public Transform targetPoint;   
    public float spawnInterval = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0; 
        }
    }

    void SpawnObject()
    {
        if (prefabToSpawn != null && targetPoint != null)
        {
            // สร้าง Object ขึ้นมาที่ตำแหน่งของ Spawner
            GameObject newObj = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            // ส่งค่าเป้าหมายไปให้ Script การเคลื่อนที่
            RatMove moveScript = newObj.GetComponent<RatMove>();
            if (moveScript != null)
            {
                moveScript.target = targetPoint;
            }
        }
    }
}
