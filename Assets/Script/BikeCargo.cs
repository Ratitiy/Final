using UnityEngine;

public class BikeCargo : MonoBehaviour
{
    public Transform cargoPoint;
    public GameObject storedItem;

    public void Interact(GameObject player)
    {
        PlayerCarry carry = player.GetComponent<PlayerCarry>();
        if (carry == null) return;

        
        if (storedItem != null && !carry.IsCarrying())
        {
            carry.PickUp(storedItem);
            storedItem = null;
            Debug.Log("หยิบของจากท้ายรถ");
            return;
        }

        
        if (storedItem == null && carry.IsCarrying())
        {
            GameObject item = carry.carriedItem;

            carry.carriedItem = null;
            storedItem = item;

            item.transform.SetParent(cargoPoint);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }

            Collider col = item.GetComponent<Collider>();
            if (col)
                col.enabled = false;

            Debug.Log("วางของไว้ท้ายรถ");
        }
    }
}