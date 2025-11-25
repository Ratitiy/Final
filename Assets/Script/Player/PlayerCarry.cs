using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    public Transform backSlot;       
    public Transform bikeSlot;       

    public OrderObject carried;

    public void Pickup(OrderObject obj)
    {
        if (carried != null) return;

        carried = obj;
        obj.PlaceAt(backSlot);
    }

    public void PlaceOnBike()
    {
        if (carried == null) return;

        carried.PlaceAt(bikeSlot);
        foreach (var r in carried.GetComponentsInChildren<Renderer>())
        {
            r.enabled = true;
        }
    }
    public void TakeOffBike()
    {
        if (carried == null) return;

        carried.PlaceAt(backSlot); 
    }

    public void Deliver(DeliveryNPC npc)
    {
        if (carried == null) return;

        Destroy(carried.gameObject);
        carried = null;

        
    }
}
