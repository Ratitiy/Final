using UnityEngine;

public class PandaDaddy : MonoBehaviour
{
    public GameObject packagePrefab;
    public Transform spawnPoint;
    public DeliveryTarget[] deliveryNPCs;

    public Transform currentTarget;

    public void Interact(GameObject player)
    {
        if (DeliveryManager.Instance.HasActiveDelivery())
        {
            Debug.Log("มีเควสอยู่แล้ว!");
            return;
        }

        GameObject package =
            Instantiate(packagePrefab, spawnPoint.position, Quaternion.identity);

        DeliveryTarget randomTarget =
            deliveryNPCs[Random.Range(0, deliveryNPCs.Length)];

        DeliveryManager.Instance.StartDelivery(package, randomTarget.transform);

        Debug.Log("ส่งไปที่: " + randomTarget.name);
    }
}
