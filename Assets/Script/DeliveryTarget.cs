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
        if (carry != null && carry.carriedItem != null)
        {
            
            RamenLogic ramen = carry.carriedItem.GetComponent<RamenLogic>();
            float qualityMultiplier = 1f;

            if (ramen != null)
            {
                
                qualityMultiplier = ramen.ramenQuality / 100f;
                Debug.Log("คุณภาพราเมงตอนส่ง: " + ramen.ramenQuality + "%");
            }

            int finalReward = Mathf.RoundToInt(DeliveryManager.Instance.rewardMoney * qualityMultiplier);

            
            MoneyManager.Instance.AddMoney(finalReward);

            carry.Drop();
            DeliveryManager.Instance.CompleteDelivery();
        }
    }

}