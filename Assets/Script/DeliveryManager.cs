using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    public GameObject currentPackage;
    public Transform currentTarget;
    public int rewardMoney = 100;

    void Awake()
    {
        Instance = this;
    }

    public void StartDelivery(GameObject package, Transform target)
    {
        currentPackage = package;
        currentTarget = target;
    }

    public bool HasActiveDelivery()
    {
        return currentPackage != null;
    }

    public void CompleteDelivery()
    {
        Destroy(currentPackage);
        currentPackage = null;
        currentTarget = null;

        MoneyManager.Instance.AddMoney(rewardMoney);
    }
}