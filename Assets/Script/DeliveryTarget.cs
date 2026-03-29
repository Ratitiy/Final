using UnityEngine;

public class DeliveryTarget : MonoBehaviour
{
    [Header("Dialog Settings")]
    public Dialog[] successDialogs;
    public Dialog[] finishDialogs;

    private bool playerInRange = false;
    private GameObject playerObj;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogManager manager = FindObjectOfType<DialogManager>();
            if (manager != null)
            {
                if (manager.isDialogActive == false)
                {
                    Debug.Log("--- เริ่มเช็คเงื่อนไขการส่งของ ---");

                    // 1. เช็คว่ามีเควสอยู่ไหม?
                    if (DeliveryManager.Instance.hasActiveDelivery)
                    {
                        Debug.Log("ผ่านด่าน 1: มีเควสส่งของอยู่");

                        // 2. เช็คว่ามาส่งถูกที่ไหม?
                        if (DeliveryManager.Instance.currentTarget == transform)
                        {
                            Debug.Log("ผ่านด่าน 2: มาส่งถูกที่เป๊ะ (เป้าหมายคือตัวนี้แหละ)");

                            // ดึงสคริปต์ PlayerCarry มาเช็คการถือของ (ลองหาจากตัวลูกเผื่อไว้ด้วย)
                            PlayerCarry carry = playerObj.GetComponent<PlayerCarry>();
                            if (carry == null) carry = playerObj.GetComponentInParent<PlayerCarry>();

                            // 3. เช็คว่าเจอระบบถือของบนตัวหมาป่าไหม?
                            if (carry != null)
                            {
                                Debug.Log("ผ่านด่าน 3: เจอสคริปต์ PlayerCarry บนตัวละคร");

                                // 4. เช็คว่า "ถือของอยู่" จริงๆ ใช่ไหม?
                                if (carry.carriedItem != null)
                                {
                                    Debug.Log("ผ่านด่าน 4: ถือของอยู่จริงๆ! -> เข้าสู่กระบวนการส่งของสำเร็จ");

                                    Dialog dialogToPlay = successDialogs.Length > 0 ? successDialogs[Random.Range(0, successDialogs.Length)] : null;
                                    if (dialogToPlay != null) manager.StartDialog(dialogToPlay);

                                    CompleteDeliveryLogic(carry);
                                }
                                else
                                {
                                    Debug.LogError("พังที่ด่าน 4: ในเกมเห็นว่ามีของบนหัว แต่ในโค้ดตัวแปร carry.carriedItem มันแจ้งว่า 'ว่างเปล่า' (ตอนสคริปต์ PlayerCarry หยิบของ อาจจะลืมตั้งค่าตัวแปรนี้ครับ!)");
                                }
                            }
                            else
                            {
                                Debug.LogError("พังที่ด่าน 3: หาสคริปต์ PlayerCarry บนตัวละครหมาป่าไม่เจอ! (ลองเช็คว่าสคริปต์นี้แปะอยู่ตรงไหน)");
                            }
                        }
                        else
                        {
                            Debug.LogError("พังที่ด่าน 2: ผิดคน! ระบบบอกให้ไปส่งที่: " + DeliveryManager.Instance.currentTarget.name + " แต่ตัวที่คุณคุยอยู่คือ: " + transform.name);
                            PlayFinishDialog(manager);
                        }
                    }
                    else
                    {
                        Debug.LogError("พังที่ด่าน 1: ไม่มีเควสให้ส่ง! (แปลว่าลืมคุยกับแพนด้า หรือเควสหายไปแล้ว)");
                        PlayFinishDialog(manager);
                    }
                }
                else
                {
                    manager.DisplayNextSentence();
                }
            }
        }
    }

    void PlayFinishDialog(DialogManager manager)
    {
        Dialog dialogToPlay = finishDialogs.Length > 0 ? finishDialogs[Random.Range(0, finishDialogs.Length)] : null;
        if (dialogToPlay != null) manager.StartDialog(dialogToPlay);
    }

    void CompleteDeliveryLogic(PlayerCarry carry)
    {
        // ... (โค้ดคำนวณเงินและคุณภาพราเมงเดิมของคุณ) ...
        RamenLogic ramen = carry.carriedItem.GetComponent<RamenLogic>();
        float qualityMultiplier = 1f;
        if (ramen != null) { qualityMultiplier = ramen.ramenQuality / 100f; }

        int finalReward = Mathf.RoundToInt(DeliveryManager.Instance.rewardMoney * qualityMultiplier);
        MoneyManager.Instance.AddMoney(finalReward);

        carry.Drop();
        DeliveryManager.Instance.CompleteDelivery();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerObj = null;
            FindObjectOfType<DialogManager>()?.EndDialog();
        }
    }
}