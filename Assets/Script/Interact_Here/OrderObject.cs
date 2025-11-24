using UnityEngine;

public class OrderObject : MonoBehaviour, IInteractable
{
    public string GetPrompt() => "กด E เพื่อหยิบของ";

    bool picked = false;



    public void Interact(GameObject player)
    {
        if (!picked)
        {
            
            var carry = player.GetComponent<PlayerCarry>();
            carry.Pickup(this);

            picked = true;
            return;
        }
    }

    public void PlaceAt(Transform t)
    {
        transform.SetParent(t);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
