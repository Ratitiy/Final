using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public Text questInfoText;

    public Color normalColor = Color.white;
    public Color failColor = Color.red;

    void Update()
    {
        if (QuestManager.Instance == null) return;

        if (QuestManager.Instance.questActive)
        {
            questInfoText.color = normalColor;

            QuestData data = QuestManager.Instance.currentQuest;
            if (data != null)
            {

                questInfoText.text = $"ภารกิจ: {data.questName}\n" +
                                     $"รางวัล: {data.rewardMoney} $";
            }
        }

        else if (QuestManager.Instance.isQuestFailed)
        {
            questInfoText.color = failColor;
            questInfoText.text = "ส่งไม่ทัน กลับไปคุยกับ\n" +"หัวหน้าเซฟ";
        }

        else
        {
            questInfoText.color = normalColor;
            questInfoText.text = "รับงานที่หัวหน้าเชฟ";
        }
    }
}