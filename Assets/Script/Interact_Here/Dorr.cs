
using UnityEngine;

public class Dorr : MonoBehaviour, IInteractable
{
    public string GetPrompt() => "[E]Enter";

    public Transform ExitPoint;

    public void Interact(GameObject player)
    {
        Debug.Log("InteractDoor");

        
        CharacterController cc = player.GetComponent<CharacterController>();

        
        if (cc != null)
        {
            cc.enabled = false;
        }

        
        player.transform.position = ExitPoint.position;

        
        if (cc != null)
        {
            cc.enabled = true;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}