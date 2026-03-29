using UnityEngine;

public class PandaDaddyDialog : MonoBehaviour
{
    // แยกกล่องข้อความออกเป็น 2 ชุด
    public Dialog startQuestDialog;   // ชุดที่ 1: คุยครั้งแรกเพื่อรับของ
    public Dialog alreadyGivenDialog; // ชุดที่ 2: คุยซ้ำตอนรับของไปแล้ว

    // ตัวแปรส่วนกลางสำหรับให้ระบบรู้ว่า "ผู้เล่นถือของอยู่ไหม?"
    public static bool hasItem = false;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogManager manager = FindObjectOfType<DialogManager>();
            if (manager != null)
            {
                if (manager.isDialogActive == false) // ถ้ากล่องข้อความปิดอยู่
                {
                    if (hasItem == false)
                    {
                        // ยังไม่เคยรับของ -> ให้พูดบทรับของ
                        manager.StartDialog(startQuestDialog);
                        hasItem = true; // อัปเดตสถานะว่าผู้เล่นรับของไปแล้ว!
                        Debug.Log("รับของจาก PandaDaddy แล้ว!");

                        // *** ถ้าคุณมีสคริปต์เสกกล่อง Cargo ให้เรียกใช้ตรงนี้ได้เลย ***
                    }
                    else
                    {
                        // รับของไปแล้ว -> ให้พูดบทคุยซ้ำ (เช่น รีบเอาไปส่งสิ!)
                        manager.StartDialog(alreadyGivenDialog);
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
            FindObjectOfType<DialogManager>()?.EndDialog();
        }
    }
}