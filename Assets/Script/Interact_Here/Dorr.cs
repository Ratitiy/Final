using System.Text;
using UnityEngine;

public class Dorr : MonoBehaviour, IInteractable
{
    public string GetPrompt() =>"[E]Enter";

    public GameObject Player;

    public Transform ExitPoint;

    public void Interact(GameObject player)
    {

        Debug.Log("InteractDoor");
        Player.transform.position = ExitPoint.position;

        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
