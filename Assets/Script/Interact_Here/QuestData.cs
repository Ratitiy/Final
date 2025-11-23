using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "DeliveryQuest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string description;

    public GameObject orderPrefab;
    public float prepareTime = 5f;

    public float rewardMoney = 200f;
}
