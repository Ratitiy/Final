using UnityEngine;

public class BikeCargo : MonoBehaviour
{
    public Transform cargoPoint;
    public GameObject storedItem;

    public void Interact(GameObject player)
    {
        PlayerCarry carry = player.GetComponent<PlayerCarry>();
        Animator anim = player.GetComponent<Animator>();

        if (carry == null) return;

        
        if (storedItem != null && !carry.IsCarrying())
        {
            RamenLogic ramen = storedItem.GetComponent<RamenLogic>();
            if (ramen != null) ramen.isOnVehicle = false;

            carry.PickUp(storedItem);
            storedItem = null;

            if (anim != null)
            {
                anim.SetBool("IsRiding", false);
                anim.SetBool("isCarrying", true);
            }
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

<<<<<<< Updated upstream
            
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.detectCollisions = true;
            }

            Collider col = item.GetComponent<Collider>();
            if (col)
            {
                col.enabled = true;
                col.isTrigger = true;
            }

=======
>>>>>>> Stashed changes
            RamenLogic ramen = item.GetComponent<RamenLogic>();
            if (ramen != null) ramen.isOnVehicle = true;

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb) { rb.isKinematic = true; rb.detectCollisions = false; }
            Collider col = item.GetComponent<Collider>();
            if (col) col.enabled = false;

            if (anim != null)
            {
                anim.SetBool("isCarrying", false);
                anim.SetBool("IsRiding", true);
                anim.SetFloat("Speed", 0);
            }
        }
    }
}