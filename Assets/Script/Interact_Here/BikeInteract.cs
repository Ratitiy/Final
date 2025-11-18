using UnityEngine;

public class BikeInteract : MonoBehaviour , IInteractable
{
    public BikeController controller;
    public string GetPrompt() => "กด E เพื่อขึ้นรถ";

    public void Interact(GameObject player)
    {
        

        controller.EnterMotorcycle(player);
    }
}
