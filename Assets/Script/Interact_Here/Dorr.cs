using UnityEngine;

public class Dorr : MonoBehaviour, IInteractable
{
    public string GetPrompt() =>"Exit";

    public GameObject Player;

    public GameObject ExitPoint;

    public void Interact(GameObject player)
    {
        player.transform.position = ExitPoint.transform.position;
        player.transform.rotation = ExitPoint.transform.rotation;
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
