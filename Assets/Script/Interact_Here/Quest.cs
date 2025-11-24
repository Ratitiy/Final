using UnityEngine;

public class Quest : MonoBehaviour, IInteractable
{
    public QuestData quest;
    public Transform spawnPoint;
    public string GetPrompt() =>"รับเควส";

    public void Interact(GameObject player)
    {
        QuestManager.Instance.StartQuest(quest, spawnPoint);

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
