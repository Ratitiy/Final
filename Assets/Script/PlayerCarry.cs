using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    public Transform holdPoint;
    public GameObject carriedItem;



    public bool IsCarrying()
    {
        return carriedItem != null;
    }

    public void PickUp(GameObject item)
    {
        if (carriedItem != null) return;

        carriedItem = item;

        item.transform.SetParent(holdPoint);
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
            col.enabled = false; //
    }

    public void Drop()
    {
        if (carriedItem == null) return;

        Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        Collider col = carriedItem.GetComponent<Collider>();
        if (col)
            col.enabled = true;

        carriedItem.transform.SetParent(null);
        carriedItem = null;
    }
}