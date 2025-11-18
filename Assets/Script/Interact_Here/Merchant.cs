using UnityEngine;

public class MerchantInteract : MonoBehaviour, IInteractable
{
    public string prompt = "กด E เพื่ออัปเกรดรถ";

    public UpgradeUI ui;             
    public BikeController targetBike;

    public string GetPrompt() => prompt;

    public void Interact(GameObject player)
    {
        ui.OpenShop(player, targetBike);
    }
}
