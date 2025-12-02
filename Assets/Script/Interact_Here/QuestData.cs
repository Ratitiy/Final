using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "DeliveryQuest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;
    

    public GameObject orderPrefab;
    public float prepareTime = 5f;

    public float rewardMoney = 200f;
    public float timeLimit = 60f;
}
