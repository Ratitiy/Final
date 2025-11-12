using UnityEngine;

public class CarInteractable : MonoBehaviour, IInteractable
{
   

    public string prompt = "กด E เพื่อขึ้นรถ";
    public Transform seat;           
    public Transform exitPoint;      
    public Transform cameraTarget;   
    public SimpleCarController controller;

    bool occupied;

    public string GetPrompt() => occupied ? "" : prompt;

    public void Interact(GameObject interactor)
    {
        if (occupied) return;
        var pvc = interactor.GetComponent<PlayerVehicleController>();
        if (!pvc) return;
        occupied = true;
        pvc.EnterVehicle(this);
    }

    public void OnFocus() { }
    public void OnLoseFocus() { }
    public void SetOccupied(bool v) => occupied = v;

}
