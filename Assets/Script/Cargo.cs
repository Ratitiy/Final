using UnityEngine;

public class Cargo : MonoBehaviour
{
    public void Interact(GameObject player)
    {
        PlayerCarry carry = player.GetComponent<PlayerCarry>();

        if (carry == null) return;

        if (!carry.IsCarrying())
        {
            carry.PickUp(gameObject);
        }
        else
        {
            Debug.Log("ถือของอยู่แล้ว!");
        }
    }
}