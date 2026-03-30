using UnityEngine;

public class Cargo : MonoBehaviour
{
    public void Interact(GameObject player)
    {
        PlayerCarry carry = player.GetComponent<PlayerCarry>();
        Animator anim = player.GetComponent<Animator>();

        if (carry == null) return;

        if (!carry.IsCarrying())
        {
            RamenLogic ramen = GetComponent<RamenLogic>();
            if (ramen != null) ramen.isOnVehicle = false;

            carry.PickUp(gameObject);

            if (anim != null) anim.SetBool("isCarrying", true);
        }
    }
}