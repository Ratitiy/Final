using UnityEngine;

public class Quest : MonoBehaviour, IInteractable
{
    public QuestData[] quests;
    public Transform spawnPoint;
    public string GetPrompt()
    {
        if (QuestManager.Instance.questActive)
        {
            return "ส่งของให้เสร็จก่อน";
        }
        return "รับเควส";
    }

    public void Interact(GameObject player)
    {
        int randomIndex = Random.Range(0, quests.Length);
        QuestData selectedQuest = quests[randomIndex];

        QuestManager.Instance.StartQuest(selectedQuest, spawnPoint);

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
