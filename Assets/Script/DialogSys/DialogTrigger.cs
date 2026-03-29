using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    private bool playerInRange = false;

    void Update()
    {
        // ถ้าผู้เล่นอยู่ในระยะ และกดปุ่ม E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // ค้นหา DialogManager
            DialogManager manager = FindObjectOfType<DialogManager>();

            // เช็คว่า ตอนนี้กล่องข้อความปิดอยู่ใช่ไหม?
            if (manager.isDialogActive == false)
            {
                manager.StartDialog(dialog); // เริ่มบทสนทนา
            }
            // แต่ถ้ากล่องข้อความเปิดอยู่แล้ว
            else
            {
                manager.DisplayNextSentence(); // แสดงประโยคถัดไป (หรือปิดกล่องถ้าข้อความหมด)
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // (เสริม) ถ้าผู้เล่นเดินหนีออกไปตอนกำลังคุย ให้กล่องข้อความปิดอัตโนมัติ
            FindObjectOfType<DialogManager>().EndDialog();
        }
    }
}