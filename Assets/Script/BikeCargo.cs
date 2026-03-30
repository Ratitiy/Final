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

        // --- ส่วนหยิบของออกจากรถ ---
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

        // --- ส่วนเอาของไปวางบนรถ (ยึดตาม Stashed changes) ---
        if (storedItem == null && carry.IsCarrying())
        {
            GameObject item = carry.carriedItem;
            carry.carriedItem = null;
            storedItem = item;

            item.transform.SetParent(cargoPoint);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            // ตั้งค่าสถานะราเมง
            RamenLogic ramen = item.GetComponent<RamenLogic>();
            if (ramen != null) ramen.isOnVehicle = true;

            // Physics: ปิดการชนตามที่เพื่อนเขียนมา (Stashed)
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false; // เพื่อนสั่งให้ปิดการตรวจจับการชน
            }

            Collider col = item.GetComponent<Collider>();
            if (col)
            {
                col.enabled = false; // เพื่อนสั่งให้ปิด Collider ไปเลย
            }

            // ตั้งค่า Animator
            if (anim != null)
            {
                anim.SetBool("isCarrying", false);
                anim.SetBool("IsRiding", true);
                anim.SetFloat("Speed", 0);
            }
        }
    }
}