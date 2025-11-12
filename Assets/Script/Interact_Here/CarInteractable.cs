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
        Debug.Log($"[CarInteractable] Interact called on {name}");
        if (occupied)
        {
            Debug.Log("[CarInteractable] รถถูกใช้งานอยู่แล้ว");
            return;
        }

        var pvc = interactor.GetComponent<PlayerVehicleController>();
        if (!pvc)
        {
            Debug.LogWarning("[CarInteractable] ไม่พบ PlayerVehicleController บน interactor");
            return;
        }

        occupied = true;
        Debug.Log("[CarInteractable] เรียก EnterVehicle()");
        pvc.EnterVehicle(this);
    }

    public void OnFocus()
    {
        Debug.Log("[CarInteractable] OnFocus()");
    }

    public void OnLoseFocus()
    {
        Debug.Log("[CarInteractable] OnLoseFocus()");
    }

    public void SetOccupied(bool v)
    {
        occupied = v;
        Debug.Log($"[CarInteractable] SetOccupied({v})");
    }
}
