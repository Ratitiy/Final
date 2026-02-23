using UnityEngine;

public class PandaDaddy : MonoBehaviour
{
    public GameObject packagePrefab;
    public Transform spawnPoint;
    public Transform[] deliveryTargets;

    public Transform currentTarget;

    public void Interact(GameObject player)
    {
        if (DeliveryManager.Instance.HasActiveDelivery())
        {
            Debug.Log("มีเควสอยู่แล้ว!");
            return;
        }

        GameObject package = Instantiate(packagePrefab, spawnPoint.position, Quaternion.identity);

        currentTarget = deliveryTargets[Random.Range(0, deliveryTargets.Length)];

        DeliveryManager.Instance.StartDelivery(package, currentTarget);
    }
}
