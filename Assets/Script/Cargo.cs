using UnityEngine;

public class Cargo : MonoBehaviour
{
    public Transform cargoPoint;
    public GameObject storedItem;

    public bool HasItem()
    {
        return storedItem != null;
    }

    public void StoreItem(GameObject item)
    {
        storedItem = item;
        item.transform.SetParent(cargoPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public GameObject TakeItem()
    {
        GameObject item = storedItem;
        storedItem = null;
        return item;
    }
}