using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("UI Reference")]
    public Text T;

    void Update()
    {
        if (QuestManager.Instance == null) return;

        if (QuestManager.Instance.questActive)
        {
            QuestData data = QuestManager.Instance.currentQuest;

            if (data != null)
            {
                T.text = $"ภารกิจ: {data.questName}\n" +
                                 $"รางวัล: {data.rewardMoney} บาท\n" +
                                 "สถานะ: กำลังไปส่ง...";
            }
        }
        else
        {
            T.text = "คุยกับเซฟ";
        }
    }
}