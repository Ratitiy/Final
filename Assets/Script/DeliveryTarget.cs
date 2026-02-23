using UnityEngine;

public class DeliveryTarget : MonoBehaviour
{
    public void Interact(GameObject player)
    {
        PlayerCarry carry = player.GetComponent<PlayerCarry>();

        if (carry == null || !carry.IsCarrying())
        {
            Debug.Log("ไม่มีของ!");
            return;
        }

        DeliveryManager.Instance.CompleteDelivery();
        carry.Drop();
    }
}