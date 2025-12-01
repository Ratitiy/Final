using UnityEngine;

public class DeliveryNPC : MonoBehaviour, IInteractable
{
    public string GetPrompt() => "กด E เพื่อส่งอาหาร";

    public void Interact(GameObject player)
    {
        var carry = player.GetComponent<PlayerCarry>();

        if (carry == null || carry.carried == null)
            return;

        Destroy(carry.carried.gameObject);
        carry.carried = null;

        QuestManager.Instance.DeliverSuccess();

        
    }
}
