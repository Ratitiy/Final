using UnityEngine;
using System.Collections;

public class MotorcycleSeat : MonoBehaviour
{
    public Transform seatPoint;
    public MotocycleV2 motorcycle;


    
    public GameObject playerCamera; 
    public GameObject bikeCamera;   


    GameObject currentPlayer;
    bool canDismount = false;

   
    public void Interact(GameObject player)
    {
        if (currentPlayer == null)
            Mount(player);
    }

    void Update()
    {
        if (currentPlayer != null && canDismount && Input.GetKeyDown(KeyCode.F))
        {
            PlayerInteract pInteract = currentPlayer.GetComponent<PlayerInteract>();
            if (pInteract != null && pInteract.currentTarget != null && pInteract.currentTarget.gameObject != this.gameObject)
            {
                pInteract.currentTarget.SendMessage("Interact", currentPlayer, SendMessageOptions.DontRequireReceiver);
                return;
            }
            Dismount();
        }
    }

    void Mount(GameObject player)
    {
        currentPlayer = player;

        
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm != null)
            pm.enabled = false;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        
        player.transform.SetParent(seatPoint);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        motorcycle.enabled = true;
    }

    IEnumerator EnableDismountCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canDismount = true;
    }

    void Dismount()
    {
        if (currentPlayer == null) return;

        
        currentPlayer.transform.SetParent(null);

        
        PlayerMovement pm = currentPlayer.GetComponent<PlayerMovement>();
        if (pm != null)
            pm.enabled = true;

        CharacterController cc = currentPlayer.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = true;

        motorcycle.enabled = false;

        currentPlayer = null;
    }
}