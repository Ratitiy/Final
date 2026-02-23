using UnityEngine;

public class DeliveryTarget : MonoBehaviour
{
    public void Interact(GameObject player)
    {
        if (!DeliveryManager.Instance.hasActiveDelivery)
            return;

        if (DeliveryManager.Instance.currentTarget != transform)
        {
            Debug.Log("ไม่ใช่จุดส่งของนี้!");
            return;
        }

        PlayerCarry carry = player.GetComponent<PlayerCarry>();

        if (carry == null || !carry.IsCarrying())
        {
            Debug.Log("ไม่มีของ!");
            return;
        }

        carry.Drop();
        DeliveryManager.Instance.CompleteDelivery();

        Debug.Log("ส่งของสำเร็จ!");
    }
}