using UnityEngine;

public class PandaDaddy : MonoBehaviour
{
    [Header("Delivery Settings")]
    public GameObject packagePrefab;  // ลาก Prefab ราเมงมาใส่
    public Transform spawnPoint;      // ลากจุดที่จะให้ราเมงโผล่มาใส่
    public DeliveryTarget[] deliveryNPCs; // ลากลูกค้า (Target) ทั้งหมดในฉากมาใส่ที่นี่ (ใส่เยอะๆ จะได้สุ่มไปหลายที่)

    [Header("Dialog Settings")]
    public Dialog[] startQuestDialogs;   // รายการบทสนทนาตอนรับเควส (ใส่หลาย Element เพื่อสุ่มชุดคำพูด)
    public Dialog[] alreadyGivenDialogs; // รายการบทสนทนาตอนทวงของ (ใส่หลาย Element เพื่อสุ่มชุดคำพูด)

    private bool playerInRange = false;

    void Update()
    {
        // เช็คว่าผู้เล่นอยู่ในระยะ และกดปุ่ม E หรือไม่
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            DialogManager manager = FindAnyObjectByType<DialogManager>();

            if (manager != null)
            {
                // ถ้าหน้าต่างคำพูดไม่ได้เปิดอยู่ -> เริ่มบทสนทนาใหม่
                if (!manager.isDialogActive)
                {
                    Dialog dialogToPlay = null;

                    // 1. เช็คว่าตอนนี้ "ไม่มี" เควสค้างอยู่ใช่ไหม?
                    if (!DeliveryManager.Instance.hasActiveDelivery)
                    {
                        // สุ่มบทสนทนาจากชุด "เริ่มเควส"
                        if (startQuestDialogs.Length > 0)
                        {
                            dialogToPlay = startQuestDialogs[Random.Range(0, startQuestDialogs.Length)];
                        }

                        // สั่งเริ่มเควส: เสกของ และ สุ่มลูกค้าเป้าหมาย
                        AssignDeliveryQuest();
                    }
                    else
                    {
                        // 2. ถ้ามีเควสค้างอยู่แล้ว -> สุ่มบทสนทนาชุด "ทวงของ"
                        if (alreadyGivenDialogs.Length > 0)
                        {
                            dialogToPlay = alreadyGivenDialogs[Random.Range(0, alreadyGivenDialogs.Length)];
                        }
                    }

                    // สั่งให้ DialogManager เริ่มแสดงผล
                    if (dialogToPlay != null)
                    {
                        manager.StartDialog(dialogToPlay);
                    }
                }
                else
                {
                    // ถ้าหน้าต่างคำพูดเปิดอยู่แล้ว -> กด E เพื่อดูประโยคถัดไป
                    manager.DisplayNextSentence();
                }
            }
        }
    }

    void AssignDeliveryQuest()
    {
        // ตรวจสอบความพร้อมของข้อมูลก่อนเริ่ม
        if (packagePrefab != null && spawnPoint != null && deliveryNPCs.Length > 0)
        {
            // 1. เสกกล่องราเมงออกมาที่จุด Spawn
            GameObject package = Instantiate(packagePrefab, spawnPoint.position, spawnPoint.rotation);

            // 2. สุ่มเลือกลูกค้า 1 คนจากรายการที่เราใส่ไว้ใน Inspector
            int randomIndex = Random.Range(0, deliveryNPCs.Length);
            DeliveryTarget selectedTarget = deliveryNPCs[randomIndex];

            // 3. ส่งข้อมูลให้ DeliveryManager เริ่มทำงาน (ส่งของชิ้นนี้ ไปที่ลูกค้าคนนี้)
            DeliveryManager.Instance.StartDelivery(package, selectedTarget.transform);
            selectedTarget.ShowMarker();

            Debug.Log("เริ่มเควสใหม่! ไปส่งของให้: " + selectedTarget.name);
        }
        else
        {
            Debug.LogError("PandaDaddy: ตั้งค่าใน Inspector ไม่ครบ! (ขาดของ, จุดเกิด หรือ รายชื่อลูกค้า)");
        }
    }

    // ระบบเช็คระยะการเข้าใกล้ (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // เดินหนีปุ๊บ ปิดกล่องคำพูดปั๊บ
            FindFirstObjectByType<DialogManager>()?.EndDialog();
        }
    }
}