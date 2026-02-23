using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    public GameObject currentPackage;
    public Transform currentTarget;
    public int rewardMoney = 100;
    public bool hasActiveDelivery = false;

    void Awake()
    {
        Instance = this;
        currentPackage = null;
        currentTarget = null;
        hasActiveDelivery = false;
    }

    public void StartDelivery(GameObject package, Transform target)
    {
        currentPackage = package;
        currentTarget = target;
        hasActiveDelivery = true;
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
        hasActiveDelivery = false;

        MoneyManager.Instance.AddMoney(rewardMoney);
    }
}