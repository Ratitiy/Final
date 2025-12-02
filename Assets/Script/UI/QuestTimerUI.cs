using UnityEngine;
using UnityEngine.UI;

public class QuestTimerUI : MonoBehaviour
{
    public GameObject BG;
    public Text timerText;          
    public Color normalColor = Color.white; 
    public Color warningColor = Color.red;  

    void Update()
    {
        if (QuestManager.Instance == null) return;
        
        bool isActive = QuestManager.Instance.questActive;
        if(isActive)
        {
            
            if (!BG.activeSelf) BG.SetActive(true);
            if (!timerText.gameObject.activeSelf) timerText.gameObject.SetActive(true);


            float time = QuestManager.Instance.remainingTime;
            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time % 60F);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            
            if (time <= 10f)
                timerText.color = warningColor;
            else
                timerText.color = normalColor;
        }
        else
        {
            
            if (BG.activeSelf)
                BG.SetActive(false);
            if (timerText.gameObject.activeSelf) timerText.gameObject.SetActive(false);
        }
    }
}