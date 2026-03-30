using UnityEngine;

public class TargetDialog : MonoBehaviour
{
    [Header("สุ่มประโยคตอนส่งของสำเร็จ")]
    public Dialog[] successDialogs;

    [Header("สุ่มประโยคตอนส่งเควสไปแล้วแต่มากดคุยซ้ำ")]
    public Dialog[] finishDialogs;

    private bool questCompleted = false;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogManager manager = FindAnyObjectByType<DialogManager>();
            if (manager != null)
            {
                if (manager.isDialogActive == false) // ถ้ากล่องข้อความปิดอยู่
                {
                    Dialog dialogToPlay = null; // ตัวแปรเตรียมรับประโยคที่สุ่มได้

                    if (questCompleted)
                    {
                        // 1. ถ้าเควสจบไปแล้ว -> สุ่มจากกล่อง finishDialogs
                        if (finishDialogs.Length > 0)
                        {
                            int randomIndex = Random.Range(0, finishDialogs.Length);
                            dialogToPlay = finishDialogs[randomIndex];
                        }
                    }
                    else if (PandaDaddyDialog.hasItem == true)
                    {
                        // 2. ถ้ามีของมาส่ง -> สุ่มจากกล่อง successDialogs
                        if (successDialogs.Length > 0)
                        {
                            int randomIndex = Random.Range(0, successDialogs.Length);
                            dialogToPlay = successDialogs[randomIndex];
                        }

                        PandaDaddyDialog.hasItem = false; // ลบของออกจากตัวผู้เล่น
                        questCompleted = true; // อัปเดตสถานะเควส
                        Debug.Log("ส่งของสำเร็จ!");
                    }
                    // *** ส่วนที่ตอนยังไม่มีของถูกตัดออกไปแล้ว ถ้าไม่มีของ dialogToPlay จะเป็น null ***

                    // สั่งให้ Manager เล่นบทสนทนาที่สุ่มได้ (ถ้ามีข้อมูล)
                    if (dialogToPlay != null)
                    {
                        manager.StartDialog(dialogToPlay);
                    }
                }
                else
                {
                    manager.DisplayNextSentence(); // กด E อ่านประโยคถัดไป
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) playerInRange = true; }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            FindAnyObjectByType<DialogManager>()?.EndDialog();
        }
    }
}