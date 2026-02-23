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
        if (motorcycle == null)
        {
            Debug.LogError("Motorcycle is not assigned in Inspector!");
            return;
        }

        if (seatPoint == null)
        {
            Debug.LogError("SeatPoint is not assigned in Inspector!");
            return;
        }

        currentPlayer = player;

        var collider = player.GetComponent<Collider>();
        if (collider) collider.enabled = false;

        player.transform.SetParent(motorcycle.transform);
        player.transform.position = seatPoint.position;
        player.transform.rotation = seatPoint.rotation;

        var move = player.GetComponent<PlayerMovement>();
        if (move) move.enabled = false;

        var cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        motorcycle.enabled = true;

        var mesh = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh) mesh.enabled = false;

        StartCoroutine(EnableDismountCooldown());
    }

    IEnumerator EnableDismountCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canDismount = true;
    }

    void Dismount()
    {
        motorcycle.enabled = false;
        currentPlayer.transform.SetParent(null);
        Vector3 dismountPos = seatPoint.position + (transform.right * -1.5f);

        RaycastHit hit;
        float groundY = transform.position.y;
        if (Physics.Raycast(dismountPos + Vector3.up * 2f, Vector3.down, out hit, 5f))
        {
            groundY = hit.point.y;
        }
        CharacterController cc = currentPlayer.GetComponent<CharacterController>();
        if (cc != null)
        {
            float heightOffset = (cc.height * 0.5f) - cc.center.y;
            dismountPos.y = groundY + heightOffset + 0.05f;
        }
        else
        {
            dismountPos.y = groundY + 1.0f;
        }

        currentPlayer.transform.position = dismountPos;
        currentPlayer.transform.rotation = Quaternion.Euler(0, currentPlayer.transform.eulerAngles.y, 0);

        var collider = currentPlayer.GetComponent<Collider>();
        if (collider) collider.enabled = true;

        currentPlayer.GetComponent<CharacterController>().enabled = true;
        if (cc != null) cc.Move(Vector3.up * 0.001f);
        if (currentPlayer.GetComponent<PlayerMovement>())
        {
            currentPlayer.GetComponent<PlayerMovement>().enabled = true;
        }

        var mesh = currentPlayer.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh) mesh.enabled = true;

        currentPlayer = null;
        canDismount = false;
    }
}