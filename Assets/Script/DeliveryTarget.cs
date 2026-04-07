using UnityEngine;

public class DeliveryTarget : MonoBehaviour
{
    [Header("Dialog Settings")]
    public Dialog[] successDialogs;
    public Dialog[] finishDialogs;

    private bool playerInRange = false;
    private GameObject playerObj;

    public GameObject minimapIcon;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            DialogManager manager = FindFirstObjectByType<DialogManager>();
            if (manager != null)
            {
                if (manager.isDialogActive == false)
                {
                    Debug.Log("--- เริ่มเช็คเงื่อนไขการส่งของ ---");

                    // 1. เช็คว่ามีเควสอยู่ไหม?
                    if (DeliveryManager.Instance.hasActiveDelivery)
                    {
                        // 2. เช็คว่ามาส่งถูกที่ไหม?
                        if (DeliveryManager.Instance.currentTarget == transform)
                        {
                            // ดึงสคริปต์ PlayerCarry มาเช็คการถือของ
                            PlayerCarry carry = playerObj.GetComponent<PlayerCarry>();
                            if (carry == null) carry = playerObj.GetComponentInParent<PlayerCarry>();

                            // 3. เช็คว่าเจอระบบถือของ และถือของอยู่จริงไหม?
                            if (carry != null && carry.carriedItem != null)
                            {
                                Debug.Log("ผ่านเงื่อนไข: กำลังส่งราเมง...");

                                // เล่นบทสนทนาสำเร็จ
                                Dialog dialogToPlay = successDialogs.Length > 0 ? successDialogs[Random.Range(0, successDialogs.Length)] : null;
                                if (dialogToPlay != null) manager.StartDialog(dialogToPlay);

                                // เข้าสู่ฟังก์ชันคำนวณเงินและจบงาน
                                CompleteDeliveryLogic(carry);
                            }
                            else
                            {
                                Debug.LogError("พังที่ด่าน 3-4: ไม่เจอ PlayerCarry หรือไม่ได้ถือของอยู่!");
                                PlayFinishDialog(manager);
                            }
                        }
                        else
                        {
                            Debug.Log("พังที่ด่าน 2: ผิดคน! ต้องไปส่งที่: " + DeliveryManager.Instance.currentTarget.name);
                            PlayFinishDialog(manager);
                        }
                    }
                    else
                    {
                        Debug.Log("พังที่ด่าน 1: ไม่มีเควสให้ส่ง!");
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
        // คำนวณคุณภาพและเงินรางวัล
        RamenLogic ramen = carry.carriedItem.GetComponent<RamenLogic>();
        float qualityMultiplier = 1f;

        if (ramen != null)
        {
            qualityMultiplier = ramen.ramenQuality / 100f;
            Debug.Log("คุณภาพราเมงตอนส่ง: " + ramen.ramenQuality + "%");
        }

        int finalReward = Mathf.RoundToInt(DeliveryManager.Instance.rewardMoney * qualityMultiplier);
        MoneyManager.Instance.AddMoney(finalReward);

        // วางของทิ้ง
        carry.Drop();

        // เคลียร์ค่า Animator (จากโค้ดฝั่ง Stashed)
        Animator anim = playerObj.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isCarrying", false);
            anim.SetBool("IsRiding", false);
        }

        // จบเควสในระบบ Manager
        DeliveryManager.Instance.CompleteDelivery();
        HideMarker();
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
            FindFirstObjectByType<DialogManager>()?.EndDialog();
        }
    }
    public void ShowMarker()
    {
        if (minimapIcon != null) minimapIcon.SetActive(true);
    }

    public void HideMarker()
    {
        if (minimapIcon != null) minimapIcon.SetActive(false);
    }
}