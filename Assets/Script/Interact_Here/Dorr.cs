using UnityEngine;

public class Dorr : MonoBehaviour, IInteractable
{
    public string GetPrompt() =>"[E]Enter";

    public GameObject Player;

    public Transform ExitPoint;

    public void Interact(GameObject player)
    {
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = ExitPoint.position;

        if (cc != null) cc.enabled = true;
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
